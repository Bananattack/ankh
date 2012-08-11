using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Defenetron {
    class GameApp:Form {
        GameApp() {

        }

        static void Main(string[] args) {
            GameApp app = new GameApp();
            Application.Run(app);
        }
    }
}
