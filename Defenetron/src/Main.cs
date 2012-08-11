using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SharpDX.Direct3D;
using SharpDX.Direct3D11;

namespace Defenetron {
    class GameApp:Form {
        private Device device;

        GameApp() {
            device = new Device(DriverType.Hardware);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //device.
        }

        static void Main(string[] args) {
            GameApp app = new GameApp();
            Application.Run(app);
        }
    }
}
