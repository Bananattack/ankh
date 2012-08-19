using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ankh.DX9
{
	public class SpriteBatch : ISpriteBatch
	{
		private readonly GraphicsDevice device;
		private readonly SharpDX.Direct3D9.Sprite sprite;

		internal SpriteBatch(GraphicsDevice device)
		{
			this.device = device;

			sprite = new SharpDX.Direct3D9.Sprite(device.Device);
			sprite.Begin(SharpDX.Direct3D9.SpriteFlags.AlphaBlend);
		}

		// FIXME: This is preposterously naive at the moment. -- andy
		public void Draw(ITexture tex)
		{
			var t = (GraphicsDevice.DX9Texture)tex;
			sprite.Draw(t.tex, new SharpDX.Rectangle(0, 0, 16, 16), new SharpDX.Vector3(0, 0, 0), new SharpDX.Vector3(0, 0, 0), SharpDX.Colors.White);
		}

		public void Dispose()
		{
			sprite.End();
			sprite.Dispose();
		}
	}
}
