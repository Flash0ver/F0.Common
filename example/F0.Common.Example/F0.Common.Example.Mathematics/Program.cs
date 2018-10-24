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

			int integer = 240;
			Console.WriteLine($"{nameof(integer)} {integer} is {(Parity.IsEven(integer) ? "" : "not ")}even.");
			Console.WriteLine($"{nameof(integer)} {integer} is {(Parity.IsOdd(integer) ? "" : "not ")}odd.");
		}
	}
}
