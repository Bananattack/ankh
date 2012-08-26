using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ankh.Core;

namespace System.IO
{
	class File
	{
		public static Stream OpenRead(string path)
		{
			return PlatformApi.Filesystem.OpenRead(path);
		}
	}
}
