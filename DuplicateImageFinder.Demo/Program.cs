using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

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
				imagePaths.AddRange(Directory.GetFiles (directoryPath, "*.JPG", SearchOption.AllDirectories));
				imagePaths.AddRange(Directory.GetFiles (directoryPath, "*.png", SearchOption.AllDirectories));
				imagePaths.AddRange(Directory.GetFiles (directoryPath, "*.PNG", SearchOption.AllDirectories));

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
			var imageCount = result.Select (c => c.Value).Count ();

			Console.WriteLine (string.Format ("{0} images processed", imageCount));

			var duplicates = result.Where (kvp => kvp.Value.Count > 1);

			if (duplicates.Count() > 0) {
				Console.WriteLine ("Found the following duplicates:");
				foreach (var item in duplicates) {
					if (item.Value.Count > 1) {
						Console.WriteLine (string.Format ("{0} ({1})", item.Key, item.Value.Count));
						foreach (var path in item.Value) {
							Console.WriteLine (string.Format ("\t{0}", path));
						}
					}
				}
			} else {
				Console.WriteLine ("No duplicates found");
			}
		}
	}
}
