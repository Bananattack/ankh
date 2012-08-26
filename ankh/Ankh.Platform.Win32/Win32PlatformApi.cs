using System;
using System.IO;
using System.Collections.Generic;

namespace Ankh.Platform.Win32
{
	internal class Win32PlatformApi : Ankh.Core.PlatformApi.IFilesystem
	{
		static Win32PlatformApi()
		{
			Ankh.Core.PlatformApi.Filesystem = new Win32PlatformApi();
		}
		public static void Initialize() { }

		Stream Ankh.Core.PlatformApi.IFilesystem.OpenRead(string path)
		{
			return File.OpenRead(path);
		}
	}
}
