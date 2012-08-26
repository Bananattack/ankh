using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ankh
{
	public struct Vector4
	{
		public float X;
		public float Y;
		public float Z;
		public float W;

		public Vector4(float x, float y, float z, float w)
		{
			X = x;
			Y = y;
			Z = z;
			W = w;
		}

		public static readonly Vector4 Zero = new Vector4(0, 0, 0, 0);
	}

	public class Vector3
	{
		public float X;
		public float Y;
		public float Z;

		public Vector3(float x, float y, float z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public static readonly Vector3 Zero = new Vector3(0, 0, 0);

		//do these with extension methods 
		//#region SharpDX specific
		//internal static Vector3 FromSharpDX(SharpDX.Vector3 v)
		//{
		//  return new Vector3(v.X, v.Y, v.Z);
		//}

		//internal SharpDX.Vector3 ToSharpDX()
		//{
		//  return new SharpDX.Vector3 { X = X, Y = Y, Z = Z };
		//}
		//#endregion
	}
}
