using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.IO
{
	static class Path
	{
		internal static void CheckInvalidPathChars(string path)
		{
			for (int i = 0; i < path.Length; i++)
			{
				int num2 = path[i];
				if (((num2 == 0x22) || (num2 == 60)) || (((num2 == 0x3e) || (num2 == 0x7c)) || (num2 < 0x20)))
				{
					throw new ArgumentException("Argument_InvalidPathChars");
				}
			}
		}


		public static readonly char DirectorySeparatorChar = '\\';
		public static readonly char AltDirectorySeparatorChar= '/';
		public static readonly char VolumeSeparatorChar= ':';

		public static string GetExtension(string path)
		{
			if (path == null)
			{
				return null;
			}
			CheckInvalidPathChars(path);
			int length = path.Length;
			int startIndex = length;
			while (--startIndex >= 0)
			{
				char ch = path[startIndex];
				if (ch == '.')
				{
					if (startIndex != (length - 1))
					{
						return path.Substring(startIndex, length - startIndex);
					}
					return string.Empty;
				}
				if (((ch == DirectorySeparatorChar) || (ch == AltDirectorySeparatorChar)) || (ch == VolumeSeparatorChar))
				{
					break;
				}
			}
			return string.Empty;
		}

 

 

	}
}
