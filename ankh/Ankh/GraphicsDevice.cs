using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Windows.Forms;

namespace Ankh
{
	public class NoDeviceException : Exception
	{
	}


	public interface ITexture
	{
		void WriteAll(int[] data);
        Vector2 Dimensions { get; }
	}

	public abstract class GraphicsDeviceBase
	{
		public GraphicsDeviceBase() {
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

		public virtual void BeginScene() { }
		public virtual void EndScene() { }
		public abstract void Present();
	}
}
