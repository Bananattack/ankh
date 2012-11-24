using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ankh.Platform.Win32.DX9
{
    public class SpriteBatch : ISpriteBatch
    {
        private readonly SharpDX.Direct3D9.Sprite sprite;

        internal SpriteBatch(GraphicsDevice device)
        {
            sprite = new SharpDX.Direct3D9.Sprite(device.Device);
            sprite.Begin(SharpDX.Direct3D9.SpriteFlags.AlphaBlend);
        }

        // FIXME: This is preposterously naive at the moment. -- andy
        public void Draw(ITexture tex, Vector3 position)
        {
            sprite.Draw(
                textureRef:  ((Texture)tex).tex,
                srcRectRef:  new SharpDX.Rectangle(0, 0, (int)tex.Dimensions.X, (int)tex.Dimensions.Y),
                centerRef:   new SharpDX.Vector3(0, 0, 0),
                positionRef: position.ToSharpDX(),
                color:       SharpDX.Colors.White
            );
        }

        public void Dispose()
        {
            sprite.End();
            sprite.Dispose();
        }
    }
}
