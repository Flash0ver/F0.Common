using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

#if HAS_READONLYSET
using TypeSet = System.Collections.Generic.IReadOnlySet<System.Type>;
#else
using TypeSet = System.Collections.Generic.ISet<System.Type>;
#endif

namespace F0.Extensions
{
	public static class TypeExtensions
	{
		private static readonly IReadOnlyDictionary<Type, string> cSharpLanguageKeywords = GetCSharpLanguageKeywords();
		private static readonly TypeSet cSharpTupleTypes = GetCSharpTupleTypes();

		public static string GetFriendlyName(this Type? type)
		{
			if (type is null)
			{
				return "null";
			}

			StringBuilder nameBuilder = new();
			SetFriendlyName(type, false, nameBuilder);
			return nameBuilder.ToString();
		}

		public static string GetFriendlyFullName(this Type? type)
		{
			if (type is null)
			{
				return "null";
			}

			StringBuilder nameBuilder = new();
			SetFriendlyName(type, true, nameBuilder);
			return nameBuilder.ToString();
		}

		private static void SetFriendlyName(Type type, bool useFullyQualifiedName, StringBuilder nameBuilder)
		{
			if (type.IsArray)
			{
				SetFriendlyArrayName(type, useFullyQualifiedName, nameBuilder);
			}
			else if (type.IsNested)
			{
				SetFriendlyNestedName(type, useFullyQualifiedName, nameBuilder);
			}
			else if (type.IsAnonymousType())
			{
				SetFriendlyAnonymousName(type, useFullyQualifiedName, nameBuilder);
			}
			else if (type.TryGetConstructedNullableValueType(out Type? underlyingValueType))
			{
				SetFriendlyNullableValueName(underlyingValueType, useFullyQualifiedName, nameBuilder);
			}
			else if (type.IsGenericType)
			{
				if (type.IsTupleType())
				{
					SetFriendlyTupleName(type, useFullyQualifiedName, nameBuilder);
				}
				else
				{
					SetFriendlyGenericName(type, useFullyQualifiedName, nameBuilder);
				}
			}
			else if (!type.IsGenericParameter)
			{
				SetFriendlyNonGenericName(type, useFullyQualifiedName, nameBuilder);
			}
		}

		private static void SetFriendlyArrayName(Type type, bool useFullyQualifiedName, StringBuilder nameBuilder)
		{
			Type? innerType = type.GetElementType();
			Debug.Assert(innerType is not null, $"IsArray={type.IsArray} of '{type}'.");
			while (innerType.IsArray)
			{
				innerType = innerType.GetElementType();
				Debug.Assert(innerType is not null, $"Current type of '{type}' is not an array.");
			}

			SetFriendlyName(innerType, useFullyQualifiedName, nameBuilder);

			Type? encompassedType = type;
			while (encompassedType.IsArray)
			{
				_ = nameBuilder.Append('[');
				_ = nameBuilder.Append(',', encompassedType.GetArrayRank() - 1);
				_ = nameBuilder.Append(']');
				encompassedType = encompassedType.GetElementType();
				Debug.Assert(encompassedType is not null, $"Current type of '{type}' is not an array.");
			}
		}

		private static void SetFriendlyNestedName(Type type, bool useFullyQualifiedName, StringBuilder nameBuilder)
		{
			if (type.IsGenericType)
			{
				Type[] genericArguments = type.GetGenericArguments();
				int offset = 0;

				Type? enclosingType = type.DeclaringType;
				if (enclosingType is not null)
				{
					ArraySegment<Type> containingGenericArguments = new(genericArguments);
					offset = SetFriendlyNestedName(enclosingType, containingGenericArguments, useFullyQualifiedName, nameBuilder);
				}

				int count = genericArguments.Length - offset;
				if (count == 0)
				{
					SetFriendlyNonGenericName(type, false, nameBuilder);
				}
				else
				{
					ArraySegment<Type> genericArgumentsSection = new(genericArguments, offset, count);
					SetFriendlyGenericName(type, genericArgumentsSection, useFullyQualifiedName, nameBuilder);
				}
			}
			else if (!type.IsGenericParameter)
			{
				string? typeName = type.FullName;
				Debug.Assert(typeName is not null, $"'{type}' is a generic type parameter or an array type.");
				string? @namespace = type.Namespace;
				if (!useFullyQualifiedName && @namespace is not null)
				{
					typeName = typeName.Substring(@namespace.Length + 1);
				}
				_ = nameBuilder.Append(typeName);
			}
		}

		private static void SetFriendlyAnonymousName(Type type, bool useFullyQualifiedName, StringBuilder nameBuilder)
		{
			_ = nameBuilder.Append('{');

			PropertyInfo[] properties = type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
			for (int i = 0; i < properties.Length; i++)
			{
				PropertyInfo property = properties[i];
				SetFriendlyName(property.PropertyType, useFullyQualifiedName, nameBuilder);

				if (i + 1 < properties.Length)
				{
					_ = nameBuilder.Append(", ");
				}
			}

			_ = nameBuilder.Append('}');
		}

		private static void SetFriendlyNullableValueName(Type underlyingValueType, bool useFullyQualifiedName, StringBuilder nameBuilder)
		{
			Debug.Assert(Nullable.GetUnderlyingType(underlyingValueType) is null, $"{underlyingValueType} should not be {nameof(Nullable)}.");

			SetFriendlyName(underlyingValueType, useFullyQualifiedName, nameBuilder);
			_ = nameBuilder.Append('?');
		}

		private static void SetFriendlyTupleName(Type type, bool useFullyQualifiedName, StringBuilder nameBuilder)
		{
			Type[] genericArguments = type.GetGenericArguments();

			_ = nameBuilder.Append('(');
			SetFriendlyGenericArguments(genericArguments, useFullyQualifiedName, nameBuilder);
			_ = nameBuilder.Append(')');
		}

		private static void SetFriendlyGenericName(Type type, bool useFullyQualifiedName, StringBuilder nameBuilder)
		{
			Type[] genericArguments = type.GetGenericArguments();
			SetFriendlyGenericName(type, new ArraySegment<Type>(genericArguments), useFullyQualifiedName, nameBuilder);
		}

		private static void SetFriendlyGenericName(Type type, ArraySegment<Type> genericArguments, bool useFullyQualifiedName, StringBuilder nameBuilder)
		{
			string mangledGenericTypeName = GetTypeName(type, !type.IsNested && useFullyQualifiedName)
				?? type.Namespace + "." + type.Name;
			string genericSeparator = $"`{genericArguments.Count}";
			int index = mangledGenericTypeName.IndexOf(genericSeparator);
			Debug.Assert(index != -1, $"String '{genericSeparator}' not found within '{mangledGenericTypeName}'.");
			string nongenericTypeName = mangledGenericTypeName.Remove(index);
			_ = nameBuilder.Append(nongenericTypeName);

			_ = nameBuilder.Append('<');
			SetFriendlyGenericArguments(genericArguments, useFullyQualifiedName, nameBuilder);
			_ = nameBuilder.Append('>');
		}

		private static void SetFriendlyNonGenericName(Type type, bool useFullyQualifiedName, StringBuilder nameBuilder)
		{
			if (cSharpLanguageKeywords.TryGetValue(type, out string? cSharpLanguageKeyword))
			{
				_ = nameBuilder.Append(cSharpLanguageKeyword);
			}
			else
			{
				string? typeName = GetTypeName(type, useFullyQualifiedName);
				Debug.Assert(!useFullyQualifiedName || typeName is not null, $"'{type}' is a generic type parameter or an array type.");
				_ = nameBuilder.Append(typeName);
			}
		}

		private static int SetFriendlyNestedName(Type type, ArraySegment<Type> genericArguments, bool useFullyQualifiedName, StringBuilder nameBuilder)
		{
			int pointer = 0;
			int length = type.GetGenericArguments().Length;

			Type? enclosingType = type.DeclaringType;
			if (enclosingType is not null)
			{
				int offset = genericArguments.Count - length;
				int count = length;
				ArraySegment<Type> genericArgumentsSection = new(genericArguments.Array!, offset, count);

				pointer = SetFriendlyNestedName(enclosingType, genericArgumentsSection, useFullyQualifiedName, nameBuilder);
				length -= pointer;
			}

			if (length == 0)
			{
				SetFriendlyNonGenericName(type, !type.IsNested && useFullyQualifiedName, nameBuilder);
			}
			else
			{
				ArraySegment<Type> genericArgumentsSection = new(genericArguments.Array!, pointer, length);
				SetFriendlyGenericName(type, genericArgumentsSection, useFullyQualifiedName, nameBuilder);
			}

			_ = nameBuilder.Append('+');

			return pointer + length;
		}

		private static void SetFriendlyGenericArguments(Type[] genericArguments, bool useFullyQualifiedName, StringBuilder nameBuilder)
		{
			SetFriendlyGenericArguments(new ArraySegment<Type>(genericArguments), useFullyQualifiedName, nameBuilder);
		}

		private static void SetFriendlyGenericArguments(ArraySegment<Type> genericArguments, bool useFullyQualifiedName, StringBuilder nameBuilder)
		{
			int length = genericArguments.Offset + genericArguments.Count;

			for (int i = genericArguments.Offset; i < length; i++)
			{
				Type genericArgument = genericArguments.Array![i];
				SetFriendlyName(genericArgument, useFullyQualifiedName, nameBuilder);

				if (i + 1 < length)
				{
					_ = genericArgument.IsGenericParameter || genericArguments.Array[i + 1].IsGenericParameter
						? nameBuilder.Append(',')
						: nameBuilder.Append(", ");
				}
			}
		}

		private static string? GetTypeName(Type type, bool useFullyQualifiedName)
		{
			return useFullyQualifiedName
				? type.FullName
				: type.Name;
		}

		private static bool IsAnonymousType(this Type type)
		{
			return Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute), false)
				&& type.IsClass
				&& type.BaseType == typeof(object)
				&& type.Namespace is null;
		}

		private static bool TryGetConstructedNullableValueType(this Type type, [NotNullWhen(true)] out Type? nullableTypeArgument)
		{
			nullableTypeArgument = Nullable.GetUnderlyingType(type);
			return nullableTypeArgument is not null;
		}

		private static bool IsTupleType(this Type type)
		{
			Type genericTypeDefinition = type.GetGenericTypeDefinition();
			return cSharpTupleTypes.Contains(genericTypeDefinition);
		}

		private static Dictionary<Type, string> GetCSharpLanguageKeywords()
		{
			return new Dictionary<Type, string>()
			{
				{ typeof(void), "void" },
				{ typeof(bool), "bool" },
				{ typeof(char), "char" },
				{ typeof(sbyte), "sbyte" },
				{ typeof(byte), "byte" },
				{ typeof(short), "short" },
				{ typeof(ushort), "ushort" },
				{ typeof(int), "int" },
				{ typeof(uint), "uint" },
				{ typeof(long), "long" },
				{ typeof(ulong), "ulong" },
				{ typeof(nint), "nint" },
				{ typeof(nuint), "nuint" },
				{ typeof(float), "float" },
				{ typeof(double), "double" },
				{ typeof(decimal), "decimal" },
				{ typeof(object), "object" },
				{ typeof(string), "string" },
			};
		}

		private static HashSet<Type> GetCSharpTupleTypes()
		{
			return new HashSet<Type>()
			{
				typeof(ValueTuple<>),
				typeof(ValueTuple<,>),
				typeof(ValueTuple<,,>),
				typeof(ValueTuple<,,,>),
				typeof(ValueTuple<,,,,>),
				typeof(ValueTuple<,,,,,>),
				typeof(ValueTuple<,,,,,,>),
				typeof(ValueTuple<,,,,,,,>),
			};
		}
	}
}
