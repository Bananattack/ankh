using System.Windows.Forms;

namespace Ankh.Platform.Win32
{
	public class Win32Game : Ankh.Game
	{
		static Win32Game()
		{
			Win32PlatformApi.Initialize();
		}
		public Win32Game(GraphicsDeviceBase device)
			: base(device)
		{
		}	
	}
}