using System.Windows.Forms;
using Ankh;

namespace Defenetron
{
	class AnkhApp : Form
	{
		private readonly Game game;

		AnkhApp()
		{
			// As of this writing, the DX11 renderer crashes when creating a Texture2D.
			//GraphicsDeviceBase device = GraphicsDevice.Create(this);
			var device = new DX9GraphicsDevice(this);
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
