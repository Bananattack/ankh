using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using SharpDX;

namespace Ankh
{
	public class GraphicsDeviceFactory
	{

		/// <summary>
		/// attempts to create a DirectX-type device, preferring 11 if available and trying 9 otherwise
		/// </summary>
		public static GraphicsDeviceBase CreateDxDevice()
		{
			try
			{
				var dx11 = new DX11GraphicsDevice();
				if (dx11.IsAvailable) return dx11;
			}
			catch
			{
			}

			try
			{
				var dx9 = new DX9GraphicsDevice();
				if (dx9.IsAvailable) return dx9;
			}
			catch
			{
			}

			return null;
		}
	}

	public interface ITexture
	{
		void WriteAll(int[] data);
	}

	public abstract class GraphicsDeviceBase
	{
		public abstract void CreateDevice(Form form);
		public abstract void ResetDevice();
		public abstract void ClearBackBuffer(Color4 color);

		//dont think these are great ideas
		public abstract int MakeVertexBuffer(int size);
		public abstract void FillVertexBuffer(int index, Vector4[] data);

		public abstract ITexture CreateTexture(int width, int height);

		public ITexture CreateTexture(Ankh.ImageIO.ArrayImage ai)
		{
			var ret = CreateTexture(ai.Width, ai.Height);
			ret.WriteAll(ai.Pixels);
			return ret;
		}

		public ITexture CreateTexture(string path)
		{
			return CreateTexture(Ankh.ImageIO.ImageIo.Load(path));
		}

		public abstract void Present();

		public abstract bool IsAvailable { get; }

		//do we really need these? i dont think i want to view them as device resources -zero
		public Color4 CreateColor(float r, float g, float b, float a = 1.0f)
		{
			return new Color4(r, g, b, a);
		}

		//do we really need these? i dont think i want to view them as device resources -zero
		public Vector4 CreateVector(float x, float y, float z, float w)
		{
			return new Vector4(x, y, z, w);
		}
	}

}
