using SharpDX;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Graphics.Display;
using Windows.UI.Core;
using D3D11 = SharpDX.Direct3D11;
using DXGI = SharpDX.DXGI;

namespace Defenetron8.Common.Modern
{
    public class Direct3DCoreWindow : IFrameworkView, IDirect3DWindow
    {
        void IFrameworkView.Initialize(CoreApplicationView applicationView)
        {
            applicationView.Activated += OnActivated;

            _device = new Direct3DDevice();
        }

        void IFrameworkView.SetWindow(CoreWindow window)
        {
            window.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 0);

            window.SizeChanged += OnWindowSizeChanged;
            window.CharacterReceived += OnCharacterReceived;
            window.KeyDown += OnKeyDown;
            window.KeyUp += OnKeyUp;
            window.PointerPressed += OnPointerPressed;
            window.PointerReleased += OnPointerReleased;
            window.PointerMoved += OnPointerMoved;
            window.Activated += OnWindowActivated;

            DisplayProperties.LogicalDpiChanged += OnLogicalDpiChanged;

            _coreWindow = window;
            _device.SetWindow(this);
        }

        void IFrameworkView.Load(string entryPoint)
        {
        }

        void IFrameworkView.Run()
        {
            CoreWindow.GetForCurrentThread().Activate();

            Render();
            _device.Present();
            CoreWindow.GetForCurrentThread().Dispatcher.ProcessEvents(CoreProcessEventsOption.ProcessUntilQuit);
        }

        private void Render()
        {
            _device.ClearBackBuffer(new Color4(123.0f / 255.0f, 160.0f / 255.0f, 183.0f / 255.0f, 1));
        }

        void IFrameworkView.Uninitialize()
        {
        }

        private void OnActivated(CoreApplicationView applicationView, IActivatedEventArgs args)
        {
            CoreWindow.GetForCurrentThread().Activate();
        }

        private void OnWindowSizeChanged(CoreWindow window, WindowSizeChangedEventArgs args)
        {
            //throw new NotImplementedException();
        }

        private void OnCharacterReceived(CoreWindow window, CharacterReceivedEventArgs args)
        {
            //throw new NotImplementedException();
        }

        private void OnKeyDown(CoreWindow window, KeyEventArgs args)
        {
            //throw new NotImplementedException();
        }

        private void OnKeyUp(CoreWindow window, KeyEventArgs args)
        {
            //throw new NotImplementedException();
        }

        private void OnPointerPressed(CoreWindow window, PointerEventArgs args)
        {
            //throw new NotImplementedException();
        }

        private void OnPointerReleased(CoreWindow window, PointerEventArgs args)
        {
            //throw new NotImplementedException();
        }

        private void OnPointerMoved(CoreWindow window, PointerEventArgs args)
        {
            //throw new NotImplementedException();
        }

        private void OnWindowActivated(CoreWindow window, WindowActivatedEventArgs args)
        {
            //throw new NotImplementedException();
        }

        private void OnLogicalDpiChanged(object sender)
        {
            // SetDpi()???
            Render();
            _device.Present();
            // handle window size change
        }

        DXGI.SwapChain1 IDirect3DWindow.CreateSwapChain(D3D11.Device1 device, ref DXGI.SwapChainDescription1 description)
        {
            var dxgiDevice = device.QueryInterface<DXGI.Device1>();
            var dxgiAdapter = dxgiDevice.Adapter;

            var dxgiFactory = dxgiAdapter.GetParent<DXGI.Factory2>();

            var coWindow = new SharpDX.ComObject(_coreWindow);
            return dxgiFactory.CreateSwapChainForCoreWindow(dxgiDevice, coWindow, ref description, null);
        }
        
        Rectangle<double> IDirect3DWindow.Bounds
        {
            get
            {
                Windows.Foundation.Rect r;

                return new Rectangle<double> { X = r.X, Y = r.Y, Width = r.Width, Height = r.Height };
            }
        }

        private Direct3DDevice _device;
        private CoreWindow _coreWindow;
    }

    public static class Direct3DCoreWindowMain 
    {
        private class FrameworkViewSource : IFrameworkViewSource
        {
            IFrameworkView IFrameworkViewSource.CreateView()
            {
                MainWindow = new Direct3DCoreWindow();
                return MainWindow;
            }
        }

        public static void Run()
        {
            var viewProviderFactory = new FrameworkViewSource();
            CoreApplication.Run(viewProviderFactory);
        }

        public static Direct3DCoreWindow MainWindow { get; private set; }
    }
}
