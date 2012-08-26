using Ankh;
using System.Windows.Forms;
using Ankh.Platform.Win32.DX11;
using Ankh.Platform.Win32.DX9;

namespace Ankh.Platform.Win32
{
	public abstract class Win32GraphicsDevice : GraphicsDeviceBase
	{
		public Form Form;

		static Win32GraphicsDevice()
		{
			Win32PlatformApi.Initialize();
		}

		/// <summary>
		/// Attempts to create a device, preferring 11 if available and trying 9 otherwise.
		/// </summary>
		public static GraphicsDeviceBase Create(Form f)
		{
			if (DX11GraphicsDevice.IsAvailable)
			{
				try
				{
					return new DX11GraphicsDevice(f);
				}
				catch
				{
				}
			}
			else
			{
				try
				{
					return new Ankh.Platform.Win32.DX9.GraphicsDevice(f);
				}
				catch
				{
				}
			}
			throw new NoDeviceException();
		}
	}
}