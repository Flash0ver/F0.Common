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
			Console.WriteLine($"{nameof(integer)} {integer} clamped to the inclusive range of {min} and {max}: {Comparable.Clamp(integer, min, max)}");

			Console.WriteLine($"The smaller {nameof(integer)} of {min} and {integer} is {Comparable.Min(min, integer)}.");
			Console.WriteLine($"The larger {nameof(integer)} of {integer} and {max} is {Comparable.Max(integer, max)}.");
		}

		private static int GetInteger(string[] args)
		{
			string? input;
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
