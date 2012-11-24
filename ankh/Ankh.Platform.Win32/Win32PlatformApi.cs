using System;
using System.IO;
using System.Collections.Generic;

namespace Ankh.Platform.Win32
{
	internal class Win32PlatformApi : Ankh.PlatformApi.IFilesystem
	{
		static Win32PlatformApi()
		{
			Ankh.PlatformApi.Filesystem = new Win32PlatformApi();
		}
		public static void Initialize() { }

		Stream Ankh.PlatformApi.IFilesystem.OpenRead(string path)
		{
			return File.OpenRead(path);
		}
	}
}
