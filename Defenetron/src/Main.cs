using System.Windows.Forms;

namespace Defenetron
{
    class AnkhApp : Form
    {
        private readonly Game TheGame;

        AnkhApp()
        {
            GraphicsDevice device = new GraphicsDevice();
            device.CreateDevice(this);

            TheGame = new DefenetronGame(device);
        }

        public void RenderLoop()
        {
            TheGame.Render();

        }

        static void Main(string[] args)
        {
            var app = new AnkhApp();
            var loop = new MessageLoop();
            loop.Run(app, app.RenderLoop);
        }
    }
}
