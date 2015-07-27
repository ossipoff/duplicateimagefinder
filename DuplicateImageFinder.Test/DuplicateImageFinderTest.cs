using System;
using System.Drawing;
using System.Linq;
using NUnit.Framework;

namespace Ossisoft
{
	[TestFixture ()]
	public class DuplicateImageFinderTest
	{
		private DictionaryImageLoader dictionaryImageLoader;
		private DuplicateImageFinder duplicateImageFinder;

		private const string aRedFilledEllipse200x200 = "aRedFilledEllipse200x200";
		private const string aRedFilledEllipse500x500 = "aRedFilledEllipse500x500";
		private const string anotherRedFilledEllipse500x500 = "anotherRedFilledEllipse500x500";
		private const string aRedFilledRectangle500x500 = "aRedFilledRectangle500x500";
		private const string aGreenFilledEllipse500x500 = "aGreenFilledEllipse500x500";

		[SetUp]
		public void Setup ()
		{
			SolidBrush brush;
			Bitmap image;
			Graphics graph;

			dictionaryImageLoader = new DictionaryImageLoader ();
			duplicateImageFinder = new DuplicateImageFinder (dictionaryImageLoader);

			brush = new SolidBrush (Color.Red);

			image = new Bitmap (200, 200);
			graph = Graphics.FromImage (image);
			graph.FillEllipse (brush, 0, 0, 200, 200);
			dictionaryImageLoader.SaveImage (aRedFilledEllipse200x200, image);

			image = new Bitmap (500, 500);
			graph = Graphics.FromImage (image);
			graph.FillEllipse (brush, 0, 0, 500, 500);
			dictionaryImageLoader.SaveImage (aRedFilledEllipse500x500, image);

			image = new Bitmap (500, 500);
			graph = Graphics.FromImage (image);
			graph.FillEllipse (brush, 0, 0, 500, 500);
			dictionaryImageLoader.SaveImage (anotherRedFilledEllipse500x500, image);

			image = new Bitmap (500, 500);
			graph = Graphics.FromImage (image);
			graph.FillRectangle (brush, new Rectangle (0, 0, 500, 500));
			dictionaryImageLoader.SaveImage (aRedFilledRectangle500x500, image);

			brush = new SolidBrush (Color.Green);

			image = new Bitmap (500, 500);
			graph = Graphics.FromImage (image);
			graph.FillEllipse (brush, 0, 0, 500, 500);
			dictionaryImageLoader.SaveImage (aGreenFilledEllipse500x500, image);
		}

		[Test ()]
		public void Two_red_ellipses_500x500_are_duplicates ()
		{
			var result = duplicateImageFinder.FindDuplicates (
				             new string[] { aRedFilledEllipse500x500, anotherRedFilledEllipse500x500 });
			
			Assert.AreEqual (1, result.Count);
			Assert.AreEqual (2, result.ElementAt (0).Value.Count);
			Assert.Contains (aRedFilledEllipse500x500, result.ElementAt (0).Value);
			Assert.Contains (anotherRedFilledEllipse500x500, result.ElementAt (0).Value);
		}

		[Test ()]
		public void Red_rectangle_500x500_and_red_ellipse_500x500_are_not_duplicates ()
		{
			var result = duplicateImageFinder.FindDuplicates (
				             new string[] { aRedFilledEllipse500x500, aRedFilledRectangle500x500 });

			Assert.AreEqual (2, result.Count);
			Assert.AreEqual (1, result.ElementAt (0).Value.Count);
			Assert.AreEqual (1, result.ElementAt (1).Value.Count);
			Assert.AreEqual (aRedFilledEllipse500x500, result.ElementAt (0).Value.ElementAt (0));
			Assert.AreEqual (aRedFilledRectangle500x500, result.ElementAt (1).Value.ElementAt (0));
		}

		[Test ()]
		public void Red_ellipse_500x500_and_red_ellipse_200x200_are_duplicates ()
		{
			var result = duplicateImageFinder.FindDuplicates (
				             new string[] { aRedFilledEllipse200x200, aRedFilledEllipse500x500 });

			Assert.AreEqual (1, result.Count);
			Assert.AreEqual (2, result.ElementAt (0).Value.Count);
			Assert.Contains (aRedFilledEllipse200x200, result.ElementAt (0).Value);
			Assert.Contains (aRedFilledEllipse500x500, result.ElementAt (0).Value);
		}

		[Test ()]
		public void Red_ellipse_500x500_and_green_ellipse_500x500_are_not_duplicates ()
		{
			var result = duplicateImageFinder.FindDuplicates (
				             new string[] { aRedFilledEllipse500x500, aGreenFilledEllipse500x500 });

			Assert.AreEqual (2, result.Count);
			Assert.AreEqual (1, result.ElementAt (0).Value.Count);
			Assert.AreEqual (1, result.ElementAt (1).Value.Count);
			Assert.AreEqual (aRedFilledEllipse500x500, result.ElementAt (0).Value.ElementAt (0));
			Assert.AreEqual (aGreenFilledEllipse500x500, result.ElementAt (1).Value.ElementAt (0));
		}
	}
}

