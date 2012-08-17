using SharpDX;
using System.Drawing;
using System.Windows.Forms;
using D3D11 = SharpDX.Direct3D11;
using DXGI = SharpDX.DXGI;

namespace Defenetron8.Common.Desktop
{
    public class Direct3DWindow : IDirect3DWindow
    {
        public Direct3DWindow()
        {
            _form = new Form();
            _form.Width = 640;
            _form.Height = 480;
            _form.BackColor = Color.Magenta;

            _device = new Direct3DDevice();
            _device.SetWindow(this);
        }

        public void Show()
        {
            _form.Show();
        }

        public void Render()
        {
            _device.ClearBackBuffer(new Color4(123.0f / 255.0f, 160.0f / 255.0f, 183.0f / 255.0f, 1));
            _device.Present();
        }

        DXGI.SwapChain1 IDirect3DWindow.CreateSwapChain(D3D11.Device1 device, ref DXGI.SwapChainDescription1 description)
        {
            var dxgiDevice = device.QueryInterface<DXGI.Device1>();
            var dxgiAdapter = dxgiDevice.Adapter;

            var dxgiFactory = dxgiAdapter.GetParent<DXGI.Factory2>();

            return dxgiFactory.CreateSwapChainForHwnd(dxgiDevice, _form.Handle, ref description, null, null);
        }

        Rectangle<double> IDirect3DWindow.Bounds
        {
            get
            {
                return new Rectangle<double> { X = 0, Y = 0, Width = 640, Height = 480 };
            }
        }

        private Form _form;
        private Direct3DDevice _device;
    }

    public static class Direct3DWindowMain
    {
        public static void Run()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            MainWindow = new Direct3DWindow();
            MainWindow.Show();

            Application.Idle += (sender, args) => MainWindow.Render();
            Application.Run();
        }

        public static Direct3DWindow MainWindow { get; private set; }
    }
}
