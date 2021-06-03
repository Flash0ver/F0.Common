using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Numerics;
using System.Threading.Tasks;
using F0.Extensions;
using F0.Tests.Shared;
using Xunit;

namespace F0.Tests.Extensions
{
	public class TypeExtensionsTests
	{
		[Fact]
		public void GetFriendlyName_Null()
		{
			Type type = null;

			string friendlyName = type.GetFriendlyName();
			string friendlyFullName = type.GetFriendlyFullName();

			Assert.Equal("null", friendlyName);
			Assert.Equal("null", friendlyFullName);
		}

		[Theory]
		[MemberData(nameof(GetTestData_CSharpBuiltInTypeName))]
		public void GetFriendlyName_CSharpBuiltInTypeName_InsteadOfFrameworkTypeNames_ForTypesThatHaveAKeywordToRepresentThem(Type type, string expected)
		{
			string friendlyName = type.GetFriendlyName();
			string friendlyFullName = type.GetFriendlyFullName();

			Assert.Equal(expected, friendlyName);
			Assert.Equal(expected, friendlyFullName);
		}

		[Theory]
		[MemberData(nameof(GetTestData_Array_CSharpBuiltInTypeName))]
		public void GetFriendlyName_Array_CSharpBuiltInTypeName(Type type, string expected)
		{
			string friendlyName = type.GetFriendlyName();
			string friendlyFullName = type.GetFriendlyFullName();

			Assert.Equal(expected, friendlyName);
			Assert.Equal(expected, friendlyFullName);
		}

		[Theory]
		[MemberData(nameof(GetTestData_Array_TypeName))]
		public void GetFriendlyName_Array_TypeName(Type type, string expectedName, string expectedFullName)
		{
			string friendlyName = type.GetFriendlyName();
			string friendlyFullName = type.GetFriendlyFullName();

			Assert.Equal(expectedName, friendlyName);
			Assert.Equal(expectedFullName, friendlyFullName);
		}

		[Theory]
		[MemberData(nameof(GetTestData_Array_Combination))]
		public void GetFriendlyName_Array_Combination(Type type, string expectedName, string expectedFullName)
		{
			string friendlyName = type.GetFriendlyName();
			string friendlyFullName = type.GetFriendlyFullName();

			Assert.Equal(expectedName, friendlyName);
			Assert.Equal(expectedFullName, friendlyFullName);
		}

		[Theory]
		[MemberData(nameof(GetTestData_NonGeneric))]
		public void GetFriendlyName_NonGeneric_TypeName(Type type)
		{
			string friendlyName = type.GetFriendlyName();
			string friendlyFullName = type.GetFriendlyFullName();

			Assert.Equal(type.Name, friendlyName);
			Assert.Equal(type.FullName, friendlyFullName);
		}

		[Theory]
		[MemberData(nameof(GetTestData_Generic_Closed))]
		public void GetFriendlyName_ClosedGeneric_TypeName(Type type, string expectedName, string expectedFullName)
		{
			Assert.True(type.IsConstructedGenericType, "generic type has type parameters (for generic type definitions)");

			string friendlyName = type.GetFriendlyName();
			string friendlyFullName = type.GetFriendlyFullName();

			Assert.Equal(expectedName, friendlyName);
			Assert.Equal(expectedFullName, friendlyFullName);
		}

		[Theory]
		[MemberData(nameof(GetTestData_Generic_Open))]
		public void GetFriendlyName_OpenGeneric_TypeName(Type type, string expectedName, string expectedFullName)
		{
			Assert.True(type.IsGenericTypeDefinition && type.ContainsGenericParameters, "generic type has type arguments (for constructed types)");

			string friendlyName = type.GetFriendlyName();
			string friendlyFullName = type.GetFriendlyFullName();

			Assert.Equal(expectedName, friendlyName);
			Assert.Equal(expectedFullName, friendlyFullName);
		}

		[Fact]
		public void GetFriendlyName_ClosedGeneric_Partially()
		{
			Type type1 = typeof(GenericType<object>).BaseType;
			Type type2 = typeof(GenericType<>).BaseType;
			Type type3 = typeof(GenericType<ValueTuple, object, object>).BaseType;
			Type type4 = typeof(GenericType<,,>).BaseType;
			Type type5 = typeof(GenericType<Action, object, DateTimeOffset, DateTimeKind>).BaseType;
			Type type6 = typeof(GenericType<,,,>).BaseType;

			Assert.False(type1.IsGenericTypeDefinition, "generic type is a generic type definition");
			Assert.False(type2.IsGenericTypeDefinition, "generic type is a generic type definition");
			Assert.False(type3.IsGenericTypeDefinition, "generic type is a generic type definition");
			Assert.False(type4.IsGenericTypeDefinition, "generic type is a generic type definition");
			Assert.False(type5.IsGenericTypeDefinition, "generic type is a generic type definition");
			Assert.False(type6.IsGenericTypeDefinition, "generic type is a generic type definition");

			string typeNamespace = "F0.Tests.Shared.";

			Assert.Equal("GenericType<object, bool>", type1.GetFriendlyName());
			Assert.Equal(typeNamespace + "GenericType<object, bool>", type1.GetFriendlyFullName());
			Assert.Equal("GenericType<,bool>", type2.GetFriendlyName());
			Assert.Equal(typeNamespace + "GenericType<,bool>", type2.GetFriendlyFullName());
			Assert.Equal("GenericType<ValueTuple, Complex>", type3.GetFriendlyName());
			Assert.Equal(typeNamespace + "GenericType<System.ValueTuple, System.Numerics.Complex>", type3.GetFriendlyFullName());
			Assert.Equal("GenericType<,Complex>", type4.GetFriendlyName());
			Assert.Equal(typeNamespace + "GenericType<,System.Numerics.Complex>", type4.GetFriendlyFullName());
			Assert.Equal("GenericType<Action, Func<DateTimeKind>, DateTimeOffset>", type5.GetFriendlyName());
			Assert.Equal(typeNamespace + "GenericType<System.Action, System.Func<System.DateTimeKind>, System.DateTimeOffset>", type5.GetFriendlyFullName());
			Assert.Equal("GenericType<,Func<>,>", type6.GetFriendlyName());
			Assert.Equal(typeNamespace + "GenericType<,System.Func<>,>", type6.GetFriendlyFullName());
		}

		[Theory]
		[MemberData(nameof(GetTestData_NestedTypes))]
		public void GetFriendlyName_NestedTypes(Type type, string expectedName, string expectedFullName)
		{
			string friendlyName = type.GetFriendlyName();
			string friendlyFullName = type.GetFriendlyFullName();

			Assert.Equal(expectedName, friendlyName);
			Assert.Equal(expectedFullName, friendlyFullName);
		}

		[Fact]
		public void GetFriendlyName_NullableValueType_GenericTypeDefinition()
		{
			Type type = typeof(Nullable<>);
			Assert.True(type.IsGenericTypeDefinition);

			string friendlyName = type.GetFriendlyName();
			string friendlyFullName = type.GetFriendlyFullName();

			Assert.Equal("Nullable<>", friendlyName);
			Assert.Equal("System.Nullable<>", friendlyFullName);
		}

		[Theory]
		[MemberData(nameof(GetTestData_NullableValueType))]
		public void GetFriendlyName_NullableValueType_Constructed(Type type, string expectedName, string expectedFullName)
		{
			string friendlyName = type.GetFriendlyName();
			string friendlyFullName = type.GetFriendlyFullName();

			Assert.Equal(expectedName, friendlyName);
			Assert.Equal(expectedFullName, friendlyFullName);
		}

		[Theory]
		[MemberData(nameof(GetTestData_Tuple))]
		public void GetFriendlyName_Tuple(Type type, string expectedName, string expectedFullName)
		{
			string friendlyName = type.GetFriendlyName();
			string friendlyFullName = type.GetFriendlyFullName();

			Assert.Equal(expectedName, friendlyName);
			Assert.Equal(expectedFullName, friendlyFullName);
		}

		[Fact]
		public void GetFriendlyName_ValueTuple_ZeroComponents()
		{
			Type type = typeof(ValueTuple);

			string friendlyName = type.GetFriendlyName();
			string friendlyFullName = type.GetFriendlyFullName();

			Assert.Equal("ValueTuple", friendlyName);
			Assert.Equal("System.ValueTuple", friendlyFullName);
		}

		[Theory]
		[MemberData(nameof(GetTestData_ValueTuple_Unbound))]
		public void GetFriendlyName_ValueTuple_Unbound(Type type, string expected)
		{
			string friendlyName = type.GetFriendlyName();
			string friendlyFullName = type.GetFriendlyFullName();

			Assert.Equal(expected, friendlyName);
			Assert.Equal(expected, friendlyFullName);
		}

		[Theory]
		[MemberData(nameof(GetTestData_ValueTuple_Bound))]
		public void GetFriendlyName_ValueTuple_Bound(Type type, string expected)
		{
			string friendlyName = type.GetFriendlyName();
			string friendlyFullName = type.GetFriendlyFullName();

			Assert.Equal(expected, friendlyName);
			Assert.Equal(expected, friendlyFullName);
		}

		[Theory]
		[MemberData(nameof(GetTestData_ValueTuple_CSharpTupleType))]
		public void GetFriendlyName_ValueTuple_CSharpTupleType(Type type, string expected)
		{
			string friendlyName = type.GetFriendlyName();
			string friendlyFullName = type.GetFriendlyFullName();

			Assert.Equal(expected, friendlyName);
			Assert.Equal(expected, friendlyFullName);
		}

		[Theory]
		[MemberData(nameof(GetTestData_GlobalNamespace))]
		public void GetFriendlyName_GlobalNamespace(Type type, string expectedName, string expectedFullName)
		{
			string friendlyName = type.GetFriendlyName();
			string friendlyFullName = type.GetFriendlyFullName();

			Assert.Equal(expectedName, friendlyName);
			Assert.Equal(expectedFullName, friendlyFullName);
		}

		[Theory]
		[MemberData(nameof(GetTestData_AnonymousType))]
		public void GetFriendlyName_AnonymousType(object anonymous, string expectedName, string expectedFullName)
		{
			Type type = anonymous.GetType();

			string friendlyName = type.GetFriendlyName();
			string friendlyFullName = type.GetFriendlyFullName();

			Assert.Equal(expectedName, friendlyName);
			Assert.Equal(expectedFullName, friendlyFullName);
		}

		public static IEnumerable<object[]> GetTestData_CSharpBuiltInTypeName()
		{
			return new object[][]
			{
				new object[] { typeof(bool), "bool" }, //System.Boolean
				new object[] { typeof(byte), "byte" }, //System.Byte
				new object[] { typeof(char), "char" }, //System.Char
				new object[] { typeof(double), "double" }, //System.Double
				new object[] { typeof(short), "short" }, //System.Int16
				new object[] { typeof(int), "int" }, //System.Int32
				new object[] { typeof(long), "long" }, //System.Int64
				new object[] { typeof(sbyte), "sbyte" }, //System.SByte
				new object[] { typeof(float), "float" }, //System.Single
				new object[] { typeof(ushort), "ushort" }, //System.UInt16
				new object[] { typeof(uint), "uint" }, //System.UInt32
				new object[] { typeof(ulong), "ulong" }, //System.UInt64

				new object[] { typeof(decimal), "decimal" }, //System.Decimal
				new object[] { typeof(object), "object" }, //System.Object
				new object[] { typeof(string), "string" }, //System.String
				new object[] { typeof(void), "void" }, //System.Void
			};
		}

		public static IEnumerable<object[]> GetTestData_Array_CSharpBuiltInTypeName()
		{
			return new object[][]
			{
				// single-dimensional array
				new object[] { typeof(int[]), "int[]" },
				new object[] { typeof(string[]), "string[]" },
				// multidimensional array
				new object[] { typeof(int[,]), "int[,]" },
				new object[] { typeof(string[,]), "string[,]" },
				// jagged array
				new object[] { typeof(int[][]), "int[][]" },
				new object[] { typeof(string[][]), "string[][]" },
			};
		}

		public static IEnumerable<object[]> GetTestData_Array_TypeName()
		{
			return new object[][]
			{
				// single-dimensional array
				new object[] { typeof(BigInteger[]), "BigInteger[]", "System.Numerics.BigInteger[]" },
				// multidimensional array
				new object[] { typeof(BigInteger[,]), "BigInteger[,]", "System.Numerics.BigInteger[,]" },
				// jagged array
				new object[] { typeof(BigInteger[][]), "BigInteger[][]", "System.Numerics.BigInteger[][]" },
			};
		}

		public static IEnumerable<object[]> GetTestData_Array_Combination()
		{
			return new object[][]
			{
				new object[] { typeof(Collection<int[]>), "Collection<int[]>", "System.Collections.ObjectModel.Collection<int[]>" },
				new object[] { typeof(Collection<int[,]>), "Collection<int[,]>", "System.Collections.ObjectModel.Collection<int[,]>" },
				new object[] { typeof(Collection<int[][]>), "Collection<int[][]>", "System.Collections.ObjectModel.Collection<int[][]>" },
				new object[] { typeof(Collection<int[,][,,]>), "Collection<int[,][,,]>", "System.Collections.ObjectModel.Collection<int[,][,,]>" },
				new object[] { typeof(ObservableCollection<int>[]), "ObservableCollection<int>[]", "System.Collections.ObjectModel.ObservableCollection<int>[]" },
				new object[] { typeof(ObservableCollection<int>[,]), "ObservableCollection<int>[,]", "System.Collections.ObjectModel.ObservableCollection<int>[,]" },
				new object[] { typeof(ObservableCollection<int>[][]), "ObservableCollection<int>[][]", "System.Collections.ObjectModel.ObservableCollection<int>[][]" },
				new object[] { typeof(ObservableCollection<int>[,][,,]), "ObservableCollection<int>[,][,,]", "System.Collections.ObjectModel.ObservableCollection<int>[,][,,]" },
				new object[] { typeof(ReadOnlyCollection<int[][,][,,]>[][,][,,]), "ReadOnlyCollection<int[][,][,,]>[][,][,,]", "System.Collections.ObjectModel.ReadOnlyCollection<int[][,][,,]>[][,][,,]" },
			};
		}

		public static IEnumerable<object[]> GetTestData_NonGeneric()
		{
			return new object[][]
			{
				new object[] { typeof(DateTime) },
				new object[] { typeof(DateTimeOffset) },
				new object[] { typeof(IntPtr) },
				new object[] { typeof(UIntPtr) },
				new object[] { typeof(BigInteger) },
				new object[] { typeof(Complex) },
				new object[] { typeof(IEnumerable) },
				new object[] { typeof(ICollection) },
				new object[] { typeof(IList) },
				new object[] { typeof(IDisposable) },
				new object[] { typeof(IAsyncDisposable) },
				new object[] { typeof(Task) },
				new object[] { typeof(ValueTask) },
				new object[] { typeof(DateTimeKind) },
				new object[] { typeof(PropertyChangedEventHandler) },
				new object[] { typeof(NotifyCollectionChangedEventHandler) },
			};
		}

		public static IEnumerable<object[]> GetTestData_Generic_Closed()
		{
			return new object[][]
			{
				new object[] { typeof(Task<int>), "Task<int>", "System.Threading.Tasks.Task<int>" },
				new object[] { typeof(ValueTask<int>), "ValueTask<int>", "System.Threading.Tasks.ValueTask<int>" },
				new object[] { typeof(IEnumerator<int>), "IEnumerator<int>", "System.Collections.Generic.IEnumerator<int>" },
				new object[] { typeof(IEnumerator<BigInteger>), "IEnumerator<BigInteger>", "System.Collections.Generic.IEnumerator<System.Numerics.BigInteger>" },
				new object[] { typeof(List<int>), "List<int>", "System.Collections.Generic.List<int>" },
				new object[] { typeof(List<Task>), "List<Task>", "System.Collections.Generic.List<System.Threading.Tasks.Task>" },
				new object[] { typeof(List<Task<int>>), "List<Task<int>>", "System.Collections.Generic.List<System.Threading.Tasks.Task<int>>" },
				new object[] { typeof(List<Task<BigInteger>>), "List<Task<BigInteger>>", "System.Collections.Generic.List<System.Threading.Tasks.Task<System.Numerics.BigInteger>>" },
				new object[] { typeof(Action<int, BigInteger>), "Action<int, BigInteger>", "System.Action<int, System.Numerics.BigInteger>" },
				new object[] { typeof(Dictionary<Type, Stack<int>>), "Dictionary<Type, Stack<int>>", "System.Collections.Generic.Dictionary<System.Type, System.Collections.Generic.Stack<int>>" },
				new object[] { typeof(Dictionary<Type, Queue<HashSet<BigInteger>>>), "Dictionary<Type, Queue<HashSet<BigInteger>>>", "System.Collections.Generic.Dictionary<System.Type, System.Collections.Generic.Queue<System.Collections.Generic.HashSet<System.Numerics.BigInteger>>>" },
			};
		}

		public static IEnumerable<object[]> GetTestData_Generic_Open()
		{
			return new object[][]
			{
				new object[] { typeof(IAsyncEnumerable<>), "IAsyncEnumerable<>", "System.Collections.Generic.IAsyncEnumerable<>" },
				new object[] { typeof(IAsyncEnumerator<>), "IAsyncEnumerator<>", "System.Collections.Generic.IAsyncEnumerator<>" },
				new object[] { typeof(Func<>), "Func<>", "System.Func<>" },
				new object[] { typeof(Func<,>), "Func<,>", "System.Func<,>" },
				new object[] { typeof(Func<,,>), "Func<,,>", "System.Func<,,>" },
				new object[] { typeof(Predicate<>), "Predicate<>", "System.Predicate<>" },
			};
		}

		public static IEnumerable<object[]> GetTestData_NestedTypes()
		{
			return new object[][]
			{
				new object[] { typeof(ContainingType.NestedType<>), "ContainingType+NestedType<>", "F0.Tests.Shared.ContainingType+NestedType<>" },
				new object[] { typeof(ContainingType.NestedType<BigInteger>), "ContainingType+NestedType<BigInteger>", "F0.Tests.Shared.ContainingType+NestedType<System.Numerics.BigInteger>" },
				new object[] { typeof(ContainingType<>.NestedType), "ContainingType<>+NestedType", "F0.Tests.Shared.ContainingType<>+NestedType" },
				new object[] { typeof(ContainingType<BigInteger>.NestedType), "ContainingType<BigInteger>+NestedType", "F0.Tests.Shared.ContainingType<System.Numerics.BigInteger>+NestedType" },
				new object[] { typeof(OuterType<>.InnerType<>), "OuterType<>+InnerType<>", "F0.Tests.Shared.OuterType<>+InnerType<>" },
				new object[] { typeof(OuterType<int>.InnerType<string>), "OuterType<int>+InnerType<string>", "F0.Tests.Shared.OuterType<int>+InnerType<string>" },
				new object[] { typeof(OuterType<ContainingType>.InnerType<ContainingType.NestedType<BigInteger>>), "OuterType<ContainingType>+InnerType<ContainingType+NestedType<BigInteger>>", "F0.Tests.Shared.OuterType<F0.Tests.Shared.ContainingType>+InnerType<F0.Tests.Shared.ContainingType+NestedType<System.Numerics.BigInteger>>" },
				new object[] { typeof(OuterType<,>.InnerType<,>), "OuterType<,>+InnerType<,>", "F0.Tests.Shared.OuterType<,>+InnerType<,>" },
				new object[] { typeof(NestingLevel1.NestingLevel2.NestingLevel3), "NestingLevel1+NestingLevel2+NestingLevel3", "F0.Tests.Shared.NestingLevel1+NestingLevel2+NestingLevel3" },
				new object[] { typeof(NestingLevel1<>.NestingLevel2<>.NestingLevel3<>), "NestingLevel1<>+NestingLevel2<>+NestingLevel3<>", "F0.Tests.Shared.NestingLevel1<>+NestingLevel2<>+NestingLevel3<>" },
				new object[] { typeof(NestingLevel1<float>.NestingLevel2<double>.NestingLevel3<decimal>), "NestingLevel1<float>+NestingLevel2<double>+NestingLevel3<decimal>", "F0.Tests.Shared.NestingLevel1<float>+NestingLevel2<double>+NestingLevel3<decimal>" },
				new object[] { typeof(NestingLevel1<,>.NestingLevel2.NestingLevel3<>), "NestingLevel1<,>+NestingLevel2+NestingLevel3<>", "F0.Tests.Shared.NestingLevel1<,>+NestingLevel2+NestingLevel3<>" },
				new object[] { typeof(NestingLevel1<float, double>.NestingLevel2.NestingLevel3<decimal>), "NestingLevel1<float, double>+NestingLevel2+NestingLevel3<decimal>", "F0.Tests.Shared.NestingLevel1<float, double>+NestingLevel2+NestingLevel3<decimal>" },
			};
		}

		public static IEnumerable<object[]> GetTestData_NullableValueType()
		{
			int? nullable = 0x_F0;
			Type boxed = nullable.GetType();

			return new object[][]
			{
				new object[] { typeof(Nullable<int>), "int?", "int?" },
				new object[] { typeof(bool?), "bool?", "bool?" },
				new object[] { typeof(char?), "char?", "char?" },
				new object[] { typeof(double?), "double?", "double?" },

				new object[] { boxed, "int", "int" },

				new object[] { typeof(int?[]), "int?[]", "int?[]" },
				new object[] { typeof(int?[,]), "int?[,]", "int?[,]" },
				new object[] { typeof(int?[][]), "int?[][]", "int?[][]" },

				new object[] { new { Number = nullable }.GetType(), "{int?}", "{int?}" },

				new object[] { typeof(Nullable<ValueTuple<char>>), "(char)?", "(char)?" },
				new object[] { typeof(ValueTuple<char?>?), "(char?)?", "(char?)?" },
				new object[] { typeof(Nullable<ValueTuple<sbyte, byte>>), "(sbyte, byte)?", "(sbyte, byte)?" },
				new object[] { typeof(Nullable<(short?, int, long)>), "(short?, int, long)?", "(short?, int, long)?" },
				new object[] { typeof(ValueTuple<ushort, uint?, ulong>?), "(ushort, uint?, ulong)?", "(ushort, uint?, ulong)?" },
				new object[] { typeof((float, double, decimal?)?), "(float, double, decimal?)?", "(float, double, decimal?)?" },

				new object[] { typeof(GlobalType<bool?>), "GlobalType<bool?>", "GlobalType<bool?>" },
				new object[] { typeof(Tuple<char?, int?, float?>), "Tuple<char?, int?, float?>", "System.Tuple<char?, int?, float?>" },
				new object[] { typeof(IEquatable<int?>), "IEquatable<int?>", "System.IEquatable<int?>" },
				new object[] { typeof(IComparable<int?>), "IComparable<int?>", "System.IComparable<int?>" },

				new object[] { typeof(ContainingType.NestedType<BigInteger?>), "ContainingType+NestedType<BigInteger?>", "F0.Tests.Shared.ContainingType+NestedType<System.Numerics.BigInteger?>" },
				new object[] { typeof(OuterType<int?>.InnerType<Complex?>), "OuterType<int?>+InnerType<Complex?>", "F0.Tests.Shared.OuterType<int?>+InnerType<System.Numerics.Complex?>" },
				new object[] { typeof(NestingLevel1<int, BigInteger?>.NestingLevel2.NestingLevel3<Complex?>), "NestingLevel1<int, BigInteger?>+NestingLevel2+NestingLevel3<Complex?>", "F0.Tests.Shared.NestingLevel1<int, System.Numerics.BigInteger?>+NestingLevel2+NestingLevel3<System.Numerics.Complex?>" },
			};
		}

		public static IEnumerable<object[]> GetTestData_Tuple()
		{
			return new object[][]
			{
				new object[] { typeof(Tuple<sbyte>), "Tuple<sbyte>", "System.Tuple<sbyte>" },
				new object[] { typeof(Tuple<sbyte, byte>), "Tuple<sbyte, byte>", "System.Tuple<sbyte, byte>" },
				new object[] { typeof(Tuple<sbyte, byte, short>), "Tuple<sbyte, byte, short>", "System.Tuple<sbyte, byte, short>" },
				new object[] { typeof(Tuple<sbyte, byte, short, ushort>), "Tuple<sbyte, byte, short, ushort>", "System.Tuple<sbyte, byte, short, ushort>" },
				new object[] { typeof(Tuple<sbyte, byte, short, ushort, int>), "Tuple<sbyte, byte, short, ushort, int>", "System.Tuple<sbyte, byte, short, ushort, int>" },
				new object[] { typeof(Tuple<sbyte, byte, short, ushort, int, uint>), "Tuple<sbyte, byte, short, ushort, int, uint>", "System.Tuple<sbyte, byte, short, ushort, int, uint>" },
				new object[] { typeof(Tuple<sbyte, byte, short, ushort, int, uint, long>), "Tuple<sbyte, byte, short, ushort, int, uint, long>", "System.Tuple<sbyte, byte, short, ushort, int, uint, long>" },
				new object[] { typeof(Tuple<sbyte, byte, short, ushort, int, uint, long, ulong>), "Tuple<sbyte, byte, short, ushort, int, uint, long, ulong>", "System.Tuple<sbyte, byte, short, ushort, int, uint, long, ulong>" },

				new object[] { typeof(Tuple<sbyte, byte, short, ushort, int, uint, long, Tuple<ulong>>), "Tuple<sbyte, byte, short, ushort, int, uint, long, Tuple<ulong>>", "System.Tuple<sbyte, byte, short, ushort, int, uint, long, System.Tuple<ulong>>" },
				new object[] { typeof(Tuple<sbyte, byte, short, ushort, int, uint, long, Tuple<ulong, float>>), "Tuple<sbyte, byte, short, ushort, int, uint, long, Tuple<ulong, float>>", "System.Tuple<sbyte, byte, short, ushort, int, uint, long, System.Tuple<ulong, float>>" },
				new object[] { typeof(Tuple<sbyte, byte, short, ushort, int, uint, long, Tuple<ulong, float, double>>), "Tuple<sbyte, byte, short, ushort, int, uint, long, Tuple<ulong, float, double>>", "System.Tuple<sbyte, byte, short, ushort, int, uint, long, System.Tuple<ulong, float, double>>" },
				new object[] { typeof(Tuple<sbyte, byte, short, ushort, int, uint, long, Tuple<ulong, float, double, decimal>>), "Tuple<sbyte, byte, short, ushort, int, uint, long, Tuple<ulong, float, double, decimal>>", "System.Tuple<sbyte, byte, short, ushort, int, uint, long, System.Tuple<ulong, float, double, decimal>>" },
			};
		}

		public static IEnumerable<object[]> GetTestData_ValueTuple_Unbound()
		{
			return new object[][]
			{
				new object[] { typeof(ValueTuple<>), "()" },
				new object[] { typeof(ValueTuple<,>), "(,)" },
				new object[] { typeof(ValueTuple<,,>), "(,,)" },
				new object[] { typeof(ValueTuple<,,,>), "(,,,)" },
				new object[] { typeof(ValueTuple<,,,,>), "(,,,,)" },
				new object[] { typeof(ValueTuple<,,,,,>), "(,,,,,)" },
				new object[] { typeof(ValueTuple<,,,,,,>), "(,,,,,,)" },
				new object[] { typeof(ValueTuple<,,,,,,,>), "(,,,,,,,)" },
			};
		}

		public static IEnumerable<object[]> GetTestData_ValueTuple_Bound()
		{
			return new object[][]
			{
				new object[] { typeof(ValueTuple<sbyte>), "(sbyte)" },
				new object[] { typeof(ValueTuple<sbyte, byte>), "(sbyte, byte)" },
				new object[] { typeof(ValueTuple<sbyte, byte, short>), "(sbyte, byte, short)" },
				new object[] { typeof(ValueTuple<sbyte, byte, short, ushort>), "(sbyte, byte, short, ushort)" },
				new object[] { typeof(ValueTuple<sbyte, byte, short, ushort, int>), "(sbyte, byte, short, ushort, int)" },
				new object[] { typeof(ValueTuple<sbyte, byte, short, ushort, int, uint>), "(sbyte, byte, short, ushort, int, uint)" },
				new object[] { typeof(ValueTuple<sbyte, byte, short, ushort, int, uint, long>), "(sbyte, byte, short, ushort, int, uint, long)" },
				new object[] { typeof(ValueTuple<sbyte, byte, short, ushort, int, uint, long, ulong>), "(sbyte, byte, short, ushort, int, uint, long, ulong)" },

				new object[] { typeof(ValueTuple<sbyte, byte, short, ushort, int, uint, long, ValueTuple<ulong>>), "(sbyte, byte, short, ushort, int, uint, long, (ulong))" },
				new object[] { typeof(ValueTuple<sbyte, byte, short, ushort, int, uint, long, ValueTuple<ulong, float>>), "(sbyte, byte, short, ushort, int, uint, long, (ulong, float))" },
				new object[] { typeof(ValueTuple<sbyte, byte, short, ushort, int, uint, long, ValueTuple<ulong, float, double>>), "(sbyte, byte, short, ushort, int, uint, long, (ulong, float, double))" },
				new object[] { typeof(ValueTuple<sbyte, byte, short, ushort, int, uint, long, ValueTuple<ulong, float, double, decimal>>), "(sbyte, byte, short, ushort, int, uint, long, (ulong, float, double, decimal))" },
			};
		}

		public static IEnumerable<object[]> GetTestData_ValueTuple_CSharpTupleType()
		{
			return new object[][]
			{
				new object[] { typeof((sbyte, byte)), "(sbyte, byte)" },
				new object[] { typeof((sbyte, byte, short)), "(sbyte, byte, short)" },
				new object[] { typeof((sbyte, byte, short, ushort)), "(sbyte, byte, short, ushort)" },
				new object[] { typeof((sbyte, byte, short, ushort, int)), "(sbyte, byte, short, ushort, int)" },
				new object[] { typeof((sbyte, byte, short, ushort, int, uint)), "(sbyte, byte, short, ushort, int, uint)" },
				new object[] { typeof((sbyte, byte, short, ushort, int, uint, long)), "(sbyte, byte, short, ushort, int, uint, long)" },
				new object[] { typeof((sbyte, byte, short, ushort, int, uint, long, ulong)), "(sbyte, byte, short, ushort, int, uint, long, (ulong))" },

				new object[] { typeof((sbyte, byte, short, ushort, int, uint, long, ulong, float)), "(sbyte, byte, short, ushort, int, uint, long, (ulong, float))" },
				new object[] { typeof((sbyte, byte, short, ushort, int, uint, long, ulong, float, double)), "(sbyte, byte, short, ushort, int, uint, long, (ulong, float, double))" },
				new object[] { typeof((sbyte, byte, short, ushort, int, uint, long, ulong, float, double, decimal)), "(sbyte, byte, short, ushort, int, uint, long, (ulong, float, double, decimal))" },

				new object[] { typeof((sbyte, byte, short, ushort, int, uint, long, (ulong, float))), "(sbyte, byte, short, ushort, int, uint, long, ((ulong, float)))" },
				new object[] { typeof((sbyte, byte, short, ushort, int, uint, long, (ulong, float, double))), "(sbyte, byte, short, ushort, int, uint, long, ((ulong, float, double)))" },
				new object[] { typeof((sbyte, byte, short, ushort, int, uint, long, (ulong, float, double, decimal))), "(sbyte, byte, short, ushort, int, uint, long, ((ulong, float, double, decimal)))" },

				new object[] { typeof((bool Boolean, char Char, object Object, string String, dynamic Dynamic)), "(bool, char, object, string, object)" },
			};
		}

		public static IEnumerable<object[]> GetTestData_GlobalNamespace()
		{
			return new object[][]
			{
				new object[] { typeof(GlobalType), "GlobalType", "GlobalType" },
				new object[] { typeof(GlobalType<>), "GlobalType<>", "GlobalType<>" },
				new object[] { typeof(GlobalType<GlobalType>), "GlobalType<GlobalType>", "GlobalType<GlobalType>" },
				new object[] { typeof(GlobalType<bool>), "GlobalType<bool>", "GlobalType<bool>" },
				new object[] { typeof(GlobalType<BigInteger>), "GlobalType<BigInteger>", "GlobalType<System.Numerics.BigInteger>" },
				new object[] { typeof(GlobalType<bool[]>), "GlobalType<bool[]>", "GlobalType<bool[]>" },
				new object[] { typeof(GlobalType<bool>[]), "GlobalType<bool>[]", "GlobalType<bool>[]" },
			};
		}

		public static IEnumerable<object[]> GetTestData_AnonymousType()
		{
			string text = "F0";
			int number = 240;
			List<KeyValuePair<bool, BigInteger>> collection = new();

			return new object[][]
			{
				new object[] { new { }, "{}", "{}" },
				new object[] { new { Text = text }, "{string}", "{string}" },
				new object[] { new { Number = number }, "{int}", "{int}" },
				new object[] { new { Array = new int[0] }, "{int[]}", "{int[]}" },
				new object[] { new { Array = new BigInteger[0, 0] }, "{BigInteger[,]}", "{System.Numerics.BigInteger[,]}" },
				new object[] { new { Array = new IEnumerable<int>[0][] }, "{IEnumerable<int>[][]}", "{System.Collections.Generic.IEnumerable<int>[][]}" },
				new object[] { new { Collection = collection }, "{List<KeyValuePair<bool, BigInteger>>}", "{System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<bool, System.Numerics.BigInteger>>}" },
				new object[] { new { Text = text, Number = number }, "{string, int}", "{string, int}" },

				new object[] { new[] { new { text, number, collection }, new { text, number, collection } }, "{string, int, List<KeyValuePair<bool, BigInteger>>}[]", "{string, int, System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<bool, System.Numerics.BigInteger>>}[]" },

				new object[] { new { Tuple = (text, number, collection) }, "{(string, int, List<KeyValuePair<bool, BigInteger>>)}", "{(string, int, System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<bool, System.Numerics.BigInteger>>)}" },
			};
		}
	}
}
