using System;
using System.Collections.Generic;
using F0.Extensions;

namespace F0.Common.Example.Extensions
{
	internal static class Program
	{
		private static void Main(string[] args)
		{
			Console.WriteLine("F0.Common");
			Console.WriteLine();

			PrettyPrint(typeof(int), "int");
			PrettyPrint(args.GetType(), "string[]");
			PrettyPrint(typeof(List<Type>.Enumerator), "List<Type>.Enumerator");
			PrettyPrint(typeof(Dictionary<,>.Enumerator), "Dictionary<,>.Enumerator");
			PrettyPrint(typeof(bool?), "bool?");
			PrettyPrint(new { Text = "F0", Number = 0x_F0 }.GetType(), @"new { Text = ""F0"", Number = 0x_F0 }");
			PrettyPrint(typeof((string Text, int Number)), "(string Text, int Number)");
		}

		private static void PrettyPrint(Type type, string cSharp)
		{
			string name = type.GetFriendlyName();
			string fullName = type.GetFriendlyFullName();

			Console.WriteLine($"- {cSharp}");
			Console.WriteLine($"  - {name}");
			Console.WriteLine($"  - {fullName}");
		}
	}
}
