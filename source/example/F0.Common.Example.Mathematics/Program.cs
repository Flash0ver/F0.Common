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

			int integer = GetInteger(args);
			Console.WriteLine($"{nameof(integer)} {integer} is {(Parity.IsEven(integer) ? "" : "not ")}even.");
			Console.WriteLine($"{nameof(integer)} {integer} is {(Parity.IsOdd(integer) ? "" : "not ")}odd.");
			Console.WriteLine();

			const int min = -240;
			const int max = +240;
			Console.WriteLine($"{nameof(integer)} {integer} clamped to the inclusive range of {min} and {max}:");
			Console.WriteLine(Comparable.Clamp(integer, min, max));
		}

		private static int GetInteger(string[] args)
		{
			string input;
			if (args.Length == 1)
			{
				input = args[0];
			}
			else
			{
				Console.Write("Enter an integer: ");
				input = Console.ReadLine();
			}

			if (!Int32.TryParse(input, out int integer))
			{
				integer = 240;
				Console.WriteLine($"'{input}' is not an {typeof(int)}. Using '{integer}'.");
			}

			return integer;
		}
	}
}
