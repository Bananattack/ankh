using System.Windows.Forms;

namespace Defenetron
{
    class GameApp : Form
    {
        private readonly GraphicsDevice _graphicsDevice;
        private readonly Game _game;

        GameApp()
        {
            GraphicsDevice device = new GraphicsDevice();
            device.CreateDevice(this);

            _game = new DefenetronGame(device);
        }

        public void RenderLoop()
        {
            _game.render();

        }

        static void Main(string[] args)
        {
            var app = new GameApp();
            var loop = new MessageLoop();
            loop.Run(app, app.RenderLoop);
        }
    }
}
