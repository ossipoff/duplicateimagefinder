using System;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Collections;
using System.Threading.Tasks;

namespace Ossisoft
{
	public class DuplicateImageFinder
	{
		const int comparisonWidth = 16;
		const int comparisonHeight = 16;

		private IImageLoader imageLoader;

		private static MD5CryptoServiceProvider md5;

		private object thisLock = new object();

		public delegate void FileProcessedEventHandler(object sender, FileProcessedEventArgs e);
		public event FileProcessedEventHandler ImageProcessed;

		static DuplicateImageFinder()
		{
			md5 = new MD5CryptoServiceProvider();
		}

		public DuplicateImageFinder()
			: this(new ImageLoader())
		{
		}

		public DuplicateImageFinder(IImageLoader imageLoader)
		{
			this.imageLoader = imageLoader;
		}

		public Dictionary<string, List<string>> FindDuplicates(IEnumerable<string> imagePaths)
		{
			Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();

			var distinctImagePaths = imagePaths.Distinct();

			Parallel.ForEach(distinctImagePaths, (imagePath) =>
			{
				string md5 = CalculateComparisonMD5(imagePath);
				lock (thisLock)
				{
					if (result.ContainsKey(md5))
					{
						result[md5].Add(imagePath);
					}
					else
					{
						result.Add(md5, new List<string>(new string[] { imagePath }));
					}

					if (ImageProcessed != null)
						ImageProcessed(this, new FileProcessedEventArgs() { FilePath = imagePath });
				}
			});

			return result;
		}

		private string CalculateComparisonMD5(string imagePath)
		{
			byte[] bytes = null;
			byte[] hash = null;

			using (var bmpOriginal = imageLoader.LoadImage(imagePath))
			{
				using (var bmpComparison = BitmapTo4bppGrayScale(ImageResize(bmpOriginal, comparisonWidth, comparisonHeight)))
				{
					using (MemoryStream ms = new MemoryStream())
					{
						bmpComparison.Save(ms, ImageFormat.Png);
						bytes = ms.ToArray();
					}
				}
			}

			lock (thisLock)
			{
				hash = md5.ComputeHash(bytes);
			}

			// make a hex string of the hash for display or whatever
			StringBuilder sb = new StringBuilder();
			foreach (byte b in hash)
			{
				sb.Append(b.ToString("x2").ToLower());
			}

			return sb.ToString();
		}

		private static Bitmap ImageResize(Image source, int newWidth, int newHeight)
		{
			int width = source.Width;
			int height = source.Height;

			Bitmap target = new Bitmap(newWidth, newHeight);
			using (Graphics graph = Graphics.FromImage(target))
			{
				graph.DrawImage(source, new Rectangle(0, 0, newWidth, newHeight), new Rectangle(0, 0, width, height), GraphicsUnit.Pixel);
			}

			return target;
		}

		private static Bitmap BitmapTo4bppGrayScale(Bitmap source)
		{
			int width = source.Width;
			int height = source.Height;
			Bitmap target = new Bitmap(width, height, PixelFormat.Format4bppIndexed);
			ColorPalette grayScalePalette = target.Palette;

			for (int i = 0; i < grayScalePalette.Entries.Length; i++)
			{
				int cval = 17 * i;
				grayScalePalette.Entries[i] = Color.FromArgb(0, cval, cval, cval);
			}

			target.Palette = grayScalePalette;

			BitmapData targetData = target.LockBits(new Rectangle(0, 0, width, height),
										ImageLockMode.ReadWrite, PixelFormat.Format4bppIndexed);

			BitmapData sourceData = source.LockBits(new Rectangle(0, 0, width, height),
										ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

			unsafe
			{
				for (int r = 0; r < height; r++)
				{
					byte* pTarget = (byte*)(targetData.Scan0 + r * targetData.Stride);
					byte* pSource = (byte*)(sourceData.Scan0 + r * sourceData.Stride);
					byte prevValue = 0;

					for (int c = 0; c < width; c++)
					{
						byte colorIndex = (byte)((((*pSource) * 0.3 + *(pSource + 1) * 0.59 + *(pSource + 2) * 0.11)) / 16);

						if (c % 2 == 0)
							prevValue = colorIndex;
						else
							*(pTarget++) = (byte)(prevValue | colorIndex << 4);

						pSource += 3;
					}
				}
			}

			target.UnlockBits(targetData);
			source.UnlockBits(sourceData);

			return target;
		}

		public interface IImageLoader
		{
			Image LoadImage(string imagePath);
		}

		public class ImageLoader : IImageLoader
		{
			#region IImageLoader implementation
			public Image LoadImage(string imagePath)
			{
				try
				{
					return new Bitmap(imagePath);
				}
				catch (Exception ex)
				{
					throw new FileLoadException(string.Format("Unable to load image '{0}'", imagePath), ex);
				}

			}
			#endregion
		}
	}

	public class FileProcessedEventArgs : EventArgs
	{
		public string FilePath { get; set; }
	}
}