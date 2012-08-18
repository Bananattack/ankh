using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Ankh.ImageIO
{
	public class ImageIo
	{
		public static ArrayImage Load(string path)
		{
			if (Path.GetExtension(path).ToUpper() == ".PNG")
			{
				var output = new PngArrayImageOutput();
				using (var f = File.OpenRead(path))
					new Png().read(f, output);
				return output.ArrayImage;
			}
			return null;
		}
	}

	public class ArrayImage
	{
		public int[] Pixels;
		public int Width;
		public int Height;
	}
}
