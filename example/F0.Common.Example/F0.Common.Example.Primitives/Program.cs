using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using F0.Primitives;

namespace F0.Common.Example.Primitives
{
	internal static class Program
	{
		private static void Main(string[] args)
		{
			Console.WriteLine("F0.Common");
			Console.WriteLine();

			IProgress<int> progressIndicator = GetProgress();

			string outputDirectory = Directory.GetCurrentDirectory();
			Console.WriteLine($"Output Directory: {outputDirectory}");

			string[] files = GetFiles(outputDirectory, progressIndicator);
			ListCollection(files);
		}

		private static IProgress<int> GetProgress()
		{
			Console.WriteLine($"Enter 'null' to use {typeof(NullProgress<>)}.");
			Console.WriteLine($"Enter 'immediate' to use {typeof(ImmediateProgress<>)}.");
			Console.Write($"Otherwise, {typeof(Progress<>)} is used: ");
			string input = Console.ReadLine();

			IProgress<int> progressIndicator;
			switch (input)
			{
				case "null":
					progressIndicator = NullProgress<int>.Instance;
					break;
				case "immediate":
					progressIndicator = new ImmediateProgress<int>(value => Console.WriteLine($"Found File #{value}"));
					break;
				default:
					progressIndicator = new Progress<int>(value => Console.WriteLine($"Found File #{value}"));
					break;
			}

			Console.WriteLine($"Using {progressIndicator.GetType()}");

			return progressIndicator;
		}

		private static string[] GetFiles(string path, IProgress<int> progress)
		{
			var files = new List<string>();

			IEnumerable<string> filePaths = Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories);

			int i = 0;
			foreach (string filePath in filePaths)
			{
				i++;
				progress.Report(i);

				string file = filePath.Replace(path, ".");
				files.Add(file);
			}

			Debug.Assert(files.Count == i);

			return files.ToArray();
		}

		private static void ListCollection(string[] collection)
		{
			foreach (string item in collection)
			{
				Console.WriteLine($"* {item}");
			}
		}
	}
}
