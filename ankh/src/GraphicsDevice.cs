using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using SharpDX;

namespace Ankh
{
	public class NoDeviceException : Exception
	{
	}

	public class GraphicsDevice
	{
		/// <summary>
		/// Attempts to create a device, preferring 11 if available and trying 9 otherwise.
		/// </summary>
		public static GraphicsDeviceBase Create(Form f)
		{
			if (DX11GraphicsDevice.IsAvailable)
			{
				try
				{
					return new DX11GraphicsDevice(f);
				}
				catch
				{
				}
			}
			else
			{
				try
				{
					return new Ankh.DX9.GraphicsDevice(f);
				}
				catch
				{
				}
			}
			throw new NoDeviceException();
		}
	}

	public interface ITexture
	{
		void WriteAll(int[] data);
	}

	public abstract class GraphicsDeviceBase
	{
		internal readonly Form Form;
		
		internal GraphicsDeviceBase(Form f) {
			Form = f;
		}
		
		public abstract void ResetDevice();
		public abstract void ClearBackBuffer(Color4 color);
		public abstract ISpriteBatch CreateSpriteBatch();

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

		internal virtual void BeginScene() { }
		internal virtual void EndScene() { }
		public abstract void Present();
	}
}
