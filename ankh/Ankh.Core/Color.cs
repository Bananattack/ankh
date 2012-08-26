using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ankh
{
	public struct Color4
	{
		public float R, G, B, A;

		public Color4(float r, float g, float b, float a)
		{
			R = r;
			G = g;
			B = b;
			A = a;
		}

		//#region SharpDX specific
		//internal static Color4 FromSharpDX(SharpDX.Color4 c)
		//{
		//  return new Color4(c.Red, c.Green, c.Blue, c.Alpha);
		//}

		//internal SharpDX.Color4 ToSharpDX()
		//{
		//  return new SharpDX.Color4(Red, Green, Blue, Alpha);
		//}
		//#endregion
	}
}
