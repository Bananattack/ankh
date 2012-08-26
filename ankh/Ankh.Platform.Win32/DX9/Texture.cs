using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3D9 = SharpDX.Direct3D9;

namespace Ankh.Platform.Win32.DX9
{
	public class Texture : ITexture
	{
		GraphicsDevice dev;
		public SharpDX.Direct3D9.Texture tex;
		public Texture(GraphicsDevice dev, int width, int height)
		{
			this.dev = dev;
			tex = new SharpDX.Direct3D9.Texture(dev.Device, width, height, 1, D3D9.Usage.Dynamic, D3D9.Format.A8R8G8B8, D3D9.Pool.Default);
		}
		public void WriteAll(int[] data)
		{
			var dr = tex.LockRectangle(0, D3D9.LockFlags.Discard);
			System.Runtime.InteropServices.Marshal.Copy(data, 0, dr.DataPointer, data.Length);
			tex.UnlockRectangle(0);
		}
	}
}
