using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace Ankh.Core
{
	public static class PlatformApi
	{
		public static IFilesystem Filesystem;


		public interface IFilesystem
		{
			Stream OpenRead(string path);
		}
	}
}
