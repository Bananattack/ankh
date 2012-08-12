
using System.Windows.Forms;
using SharpDX;


namespace Defenetron
{
    class GameApp : Form
    {
        private readonly GraphicsDevice _graphicsDevice;
        private readonly Game _game;

        GameApp()
        {
            _game = new Game();

            _graphicsDevice = new GraphicsDevice();
            _graphicsDevice.CreateDevice(this);
        }

        public void RenderLoop()
        {
            _game.render();

            _graphicsDevice.ClearBackBuffer(
                new Color4(123.0f / 255.0f, 160.0f / 255.0f, 183.0f / 255.0f, 1));

            _graphicsDevice.Present();
        }

        static void Main(string[] args)
        {
            var app = new GameApp();
            var loop = new MessageLoop();
            loop.Run(app, app.RenderLoop);
        }
    }
}
