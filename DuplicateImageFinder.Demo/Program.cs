using System;
using System.IO;
using System.Collections.Generic;

namespace DuplicateImageFinder.Demo
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			if (args.Length > 0) {
				string directoryPath = args [0];

				if (!Directory.Exists (directoryPath)) {
					WriteUsageInfo ("Passed directory does not exist!"); return;
				}

				List<string> imagePaths = new List<string> ();

				imagePaths.AddRange(Directory.GetFiles (directoryPath, "*.jpg", SearchOption.AllDirectories));
				imagePaths.AddRange(Directory.GetFiles (directoryPath, "*.png", SearchOption.AllDirectories));

				if (imagePaths.Count == 0) {
					WriteUsageInfo ("No .jpg or .png files in directory or sub directories!"); return;
				}

				var result = new Ossisoft.DuplicateImageFinder ().FindDuplicates (imagePaths);

				WriteResult (result);

			} else {
				WriteUsageInfo ("No arguments passed!"); return;
			}
		}

		private static void WriteUsageInfo(string message) {
			if (!string.IsNullOrEmpty (message)) {
				Console.WriteLine (message);
			}
			Console.WriteLine ("Usage: DuplicateImageFinder.Demo /path/to/image/directory");
		}

		private static void WriteResult(Dictionary<string, List<string>> result) {
			foreach (var item in result) {
				Console.WriteLine (string.Format ("{0} ({1})", item.Key, item.Value.Count));
				foreach (var path in item.Value) {
					Console.WriteLine (string.Format ("\t{0}", path));
				}
			}
		}
	}
}
