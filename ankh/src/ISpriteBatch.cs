using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ankh
{
	public interface ISpriteBatch : IDisposable
	{
		void Draw(ITexture tex, Vector3 position);
	}
}
