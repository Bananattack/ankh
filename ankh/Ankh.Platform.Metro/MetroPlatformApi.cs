using System;
using System.IO;
using System.Collections.Generic;

namespace Ankh.Platform.Metro
{
    internal class MetroPlatformApi : Ankh.Core.PlatformApi.IFilesystem
    {
        static MetroPlatformApi()
        {
            Ankh.Core.PlatformApi.Filesystem = new MetroPlatformApi();
        }
        public static void Initialize() { }

        T Run<T>(Windows.Foundation.IAsyncOperation<T> item)
        {
            var task = item.AsTask();
            task.Wait();
            return task.Result;
        }

        Stream Ankh.Core.PlatformApi.IFilesystem.OpenRead(string path)
        {
            path = path.Replace('/', '\\');
            var file = Run(Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync(path));
            var stream = Run(file.OpenReadAsync());
            return stream.AsStreamForRead();
        }
    }
}
