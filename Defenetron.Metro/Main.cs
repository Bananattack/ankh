using Ankh.Platform.Metro;

namespace Defenetron.Metro
{
    class DefenetronMetroMain
    {
        public static void Main(string[] args)
        {
            var device = new Ankh.Platform.Metro.MetroGraphicsDevice();
            var game = new DefenetronGame(device);
            Direct3DCoreWindowMain.Run(game);
        }
    }
}
