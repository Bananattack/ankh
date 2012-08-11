using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using Device = SharpDX.Direct3D11.Device;

namespace Defenetron {
    class GameApp:Form {
        private Device _device;
        private SwapChain _swapChain;

        GameApp() {
            //Device.CreateWithSwapChain()
            
            var scDesc = new SwapChainDescription();
            scDesc.BufferCount = 2;
            scDesc.Flags = SwapChainFlags.AllowModeSwitch;
            scDesc.IsWindowed = true;
            scDesc.ModeDescription = 
                new ModeDescription(
                    ClientSize.Width, 
                    ClientSize.Height, 
                    new Rational(60,1), 
                    Format.R8G8B8A8_UNorm);
            scDesc.OutputHandle = this.Handle;
            scDesc.SampleDescription = new SampleDescription(1, 0);
            scDesc.SwapEffect = SwapEffect.Sequential;
            scDesc.Usage = Usage.RenderTargetOutput;

            var levels = new []{FeatureLevel.Level_9_2, FeatureLevel.Level_9_1};

            Device.CreateWithSwapChain(
                DriverType.Hardware, 
                DeviceCreationFlags.None, 
                levels, 
                scDesc, 
                out _device,
                out _swapChain);
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
