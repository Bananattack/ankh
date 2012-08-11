using System;
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
            var scDesc = new SwapChainDescription
                             {
                                 BufferCount = 2,
                                 Flags = SwapChainFlags.AllowModeSwitch,
                                 IsWindowed = true,
                                 ModeDescription = new ModeDescription(
                                     ClientSize.Width,
                                     ClientSize.Height,
                                     new Rational(60, 1),
                                     Format.R8G8B8A8_UNorm),
                                 OutputHandle = Handle,
                                 SampleDescription = new SampleDescription(1, 0),
                                 SwapEffect = SwapEffect.Sequential,
                                 Usage = Usage.RenderTargetOutput
                             };

            var levels = new []{FeatureLevel.Level_9_2, FeatureLevel.Level_9_1};

            Device.CreateWithSwapChain(
                DriverType.Hardware, 
                DeviceCreationFlags.None, 
                levels, 
                scDesc, 
                out _device,
                out _swapChain);
        }

        public void RenderLoop()
        {

        }

        static void Main(string[] args) {
            var app = new GameApp();
            var loop = new MessageLoop();
            loop.Run(app, app.RenderLoop);
        }
    }
}
