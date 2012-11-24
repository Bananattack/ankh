using System.Windows.Forms;
using Ankh;

namespace Defenetron
{
    class AnkhApp
    {
        private AnkhApp() { }

        static void Main(string[] args)
        {
            var frame = new Form();
            var device = new Ankh.Platform.Win32.DX9.GraphicsDevice(frame);
            //var device = Ankh.Platform.Win32.Win32GraphicsDevice.Create(frame); //need spritebatch
            var game = new DefenetronGame(device);
            var m = new Ankh.Platform.Win32.MessageLoop();
            game.Setup();
            m.Run(game);
        }
    }
}
