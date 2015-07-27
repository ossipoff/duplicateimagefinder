using System;
using System.Drawing;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text;
using System.Linq;
using System.Drawing.Imaging;
using System.Collections.Generic;

namespace Ossisoft
{
	public class DictionaryImageLoader : DuplicateImageFinder.IImageLoader
	{
		Dictionary<string, Bitmap> bitmapDictionary = new Dictionary<string, Bitmap> ();

		public void SaveImage (string imagePath, Bitmap bitmap)
		{
			if (bitmapDictionary.ContainsKey (imagePath)) {
				bitmapDictionary [imagePath] = bitmap;
			} else {
				bitmapDictionary.Add (imagePath, bitmap);
			}
		}

		#region IImageLoader implementation

		public Image LoadImage (string imagePath)
		{
			if (!bitmapDictionary.ContainsKey (imagePath)) {
				throw new FileNotFoundException (string.Format ("", imagePath));
			}

			return bitmapDictionary [imagePath];
		}

		#endregion
	}
}

