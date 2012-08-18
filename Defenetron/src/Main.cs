using System.Windows.Forms;
using Ankh;

namespace Defenetron
{
	class AnkhApp : Form
	{
		private readonly Game game;

		AnkhApp()
		{
			GraphicsDeviceBase device = GraphicsDeviceFactory.CreateDxDevice();
			device.CreateDevice(this);

			game = new DefenetronGame(device);
		}

		public void RenderLoop()
		{
			game.Render();

		}

		static void Main(string[] args)
		{
			var app = new AnkhApp();
			var loop = new MessageLoop();
			loop.Run(app, app.RenderLoop);
		}
	}
}
