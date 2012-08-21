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
			var device = new Ankh.DX9.GraphicsDevice(frame);
			var game = new DefenetronGame(device);
			var m = new MessageLoop();
			m.Run(game);
		}
	}
}
