using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

//note - sysdrawing dependency should be removed eventually, its just for testing now

namespace Ankh.ImageIO
{
	class test
	{
		static void Main(string[] args)
		{
			//could test all pngs this way
			//foreach (var fi in new DirectoryInfo("../../test/ImageIo/pngsuite").GetFiles("*.png"))
			//{
			//  var output = new SysdrawingImageOutput();
			//  using(var f = fi.OpenRead())
			//    new Png().read(f, output);
			//}

			var output = new ArrayImageOutput();
			using (var f = File.OpenRead("../../test/ImageIo/32bpp.png"))
				new Png().read(f, output);
			using (var bmp = new System.Drawing.Bitmap("../../test/ImageIo/32bpp.png"))
				for (int y = 0; y < bmp.Height; y++)
					for (int x = 0; x < bmp.Width; x++)
						if (bmp.GetPixel(x, y).ToArgb() != output.ArrayImage.Pixels[y*bmp.Width+x])
							throw new InvalidOperationException();
				
		}
	}
}
