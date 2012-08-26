namespace Ankh.Platform.Metro
{
    public static class PlatformWin32Extensions
    {
        public static SharpDX.Vector3 ToSharpDX(this Vector3 vec3)
        {
            return new SharpDX.Vector3 { X = vec3.X, Y = vec3.Y, Z = vec3.Z };
        }

        public static SharpDX.Color4 ToSharpDX(this Color4 col4)
        {
            return new SharpDX.Color4(col4.R, col4.G, col4.B, col4.A);
        }

        public static Color4 ToAnkh(this SharpDX.Color4 col4)
        {
            return new Color4(col4.Red, col4.Green, col4.Blue, col4.Alpha);
        }
    }
}