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

			IProgress<int> progressIndicator = GetProgress(args);

			string outputDirectory = Directory.GetCurrentDirectory();
			Console.WriteLine($"Output Directory: {outputDirectory}");

			string[] files = GetFiles(outputDirectory, progressIndicator);
			ListCollection(files);
		}

		private static IProgress<int> GetProgress(string[] args)
		{
			string input;
			if (args.Length == 1)
			{
				input = args[0];
			}
			else
			{
				Console.WriteLine($"Enter 'null' to use {typeof(NullProgress<>)}.");
				Console.WriteLine($"Enter 'immediate' to use {typeof(ImmediateProgress<>)}.");
				Console.Write($"Otherwise, {typeof(Progress<>)} is used: ");
				input = Console.ReadLine();
			}

			IProgress<int> progressIndicator = input switch
			{
				"null" => NullProgress<int>.Instance,
				"immediate" => new ImmediateProgress<int>(value => Console.WriteLine($"Found File #{value}")),
				_ => new Progress<int>(value => Console.WriteLine($"Found File #{value}")),
			};
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
