using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace Ankh.ImageIO
{
	public interface IPngImageOutput
	{
		void Start(Png png);
		void WriteLine(byte[] data, int offset);
		void Finish();
	}

	class ArrayImageOutput : IPngImageOutput
	{
		public ArrayImage ArrayImage;
		public void Start(Png png)
		{
			int width = png.ihdr.width, height = png.ihdr.height, bitdepth = png.ihdr.bitdepth;
			ColorType colortype = png.ihdr.colortype;
			switch (colortype)
			{
				case ColorType.GRAY:
					throw new NotSupportedException(); //support these later
				case ColorType.GRAY_ALPHA:
					throw new NotSupportedException(); //support these later
				case ColorType.PALETTE:
					throw new NotSupportedException(); //support these later
				case ColorType.RGB:
					if (bitdepth == 16) throw new NotSupportedException();
					else pf = PixelFormat.Format24bppRgb;
					break;
				case ColorType.RGB_ALPHA:
					if (bitdepth == 16) throw new NotSupportedException();
					else pf = PixelFormat.Format32bppArgb;
					break;
			}

			ArrayImage = new ArrayImage();
			ArrayImage.Width = width;
			ArrayImage.Height = height;
			ArrayImage.Pixels = new int[width * height];
		}

		public void Finish()
		{
		}

		public void WriteLine(byte[] data, int offset)
		{
			int didx = ArrayImage.Width * linecounter;
			switch (pf)
			{
				case PixelFormat.Format32bppArgb:
					{
						//swap r and b
						for (int i = 0, x = 0; i < ArrayImage.Width; i++, x += 4)
						{
							int r = data[x + offset + 2];
							int g = data[x + offset + 1];
							int b = data[x + offset];
							int a = data[x + offset + 3];
							ArrayImage.Pixels[didx + i] = (a << 24) | (b << 16) | (g << 8) | r;
						}
						break;
					}
				case PixelFormat.Format24bppRgb:
					{
						//swap r and b
						for (int i = 0, x = 0; i < ArrayImage.Width; i++, x += 3)
						{
							int r = data[x + offset + 2];
							int g = data[x + offset + 1];
							int b = data[x + offset];
							int a = 0xFF;
							ArrayImage.Pixels[didx + i] = (a << 24) | (b << 16) | (g << 8) | r;
						}
						break;
					}
			}
			linecounter++;
		}

		int linecounter;
		PixelFormat pf;
	}

	class SysdrawingImageOutput : IPngImageOutput
	{

		public void Start(Png png)
		{
			int width = png.ihdr.width, height = png.ihdr.height, bitdepth = png.ihdr.bitdepth;
			ColorType colortype = png.ihdr.colortype;
			switch (colortype)
			{
				case ColorType.GRAY:
					throw new NotSupportedException(); //support these later
				case ColorType.GRAY_ALPHA:
					throw new NotSupportedException(); //support these later
				case ColorType.PALETTE:
					throw new NotSupportedException(); //support these later
				case ColorType.RGB:
					if (bitdepth == 16) throw new NotSupportedException();
					else pf = PixelFormat.Format24bppRgb;
					break;
				case ColorType.RGB_ALPHA:
					if(bitdepth == 16) throw new NotSupportedException();
					else pf = PixelFormat.Format32bppArgb;
					break;
			}

			bmp = new Bitmap(width, height, pf);
			bmpdata = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, pf);
		}

		public void Finish()
		{
			bmp.UnlockBits(bmpdata);
		}

		public void WriteLine(byte[] data, int offset)
		{
			IntPtr lineptr = new IntPtr(bmpdata.Scan0.ToInt32() + linecounter * bmpdata.Stride);
			switch (pf)
			{
				case PixelFormat.Format32bppArgb:
					{
						//swap r and b
						byte[] temp = new byte[bmp.Width * 4];
						for (int i = 0, x = 0; i < bmp.Width; i++, x += 4)
						{
							temp[x] = data[x + offset + 2];
							temp[x + 1] = data[x + offset + 1];
							temp[x + 2] = data[x + offset];
							temp[x + 3] = data[x + offset + 3];
						}
						System.Runtime.InteropServices.Marshal.Copy(temp, 0, lineptr, bmp.Width * 4);
						break;
					}
				case PixelFormat.Format24bppRgb:
					{
						//swap r and b
						byte[] temp = new byte[bmp.Width * 3];
						for (int i = 0, x = 0; i < bmp.Width; i++, x += 3)
						{
							temp[x] = data[x + offset + 2];
							temp[x + 1] = data[x + offset + 1];
							temp[x + 2] = data[x + offset];
						}
						System.Runtime.InteropServices.Marshal.Copy(temp, 0, lineptr, bmp.Width * 3);
						break;
					}
			}
			linecounter++;
		}

		int linecounter;
		public Bitmap bmp;
		PixelFormat pf;
		BitmapData bmpdata;
	}
}
