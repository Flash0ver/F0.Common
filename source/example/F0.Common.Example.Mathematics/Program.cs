using System;
using F0.Mathematics;

namespace F0.Common.Example.Mathematics
{
	internal static class Program
	{
		private static void Main(string[] args)
		{
			Console.WriteLine("F0.Common");
			Console.WriteLine();

			int integer = GetInteger();
			Console.WriteLine($"{nameof(integer)} {integer} is {(Parity.IsEven(integer) ? "" : "not ")}even.");
			Console.WriteLine($"{nameof(integer)} {integer} is {(Parity.IsOdd(integer) ? "" : "not ")}odd.");
		}

		private static int GetInteger()
		{
			Console.Write("Enter an integer: ");
			string input = Console.ReadLine();

			if (!Int32.TryParse(input, out int integer))
			{
				integer = 240;
				Console.WriteLine($"'{input}' is not an {typeof(int)}. Using '{integer}'.");
			}

			return integer;
		}
	}
}
