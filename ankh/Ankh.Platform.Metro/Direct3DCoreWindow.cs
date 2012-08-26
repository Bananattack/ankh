using SharpDX;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Graphics.Display;
using Windows.UI.Core;
using D3D11 = SharpDX.Direct3D11;
using DXGI = SharpDX.DXGI;

namespace Ankh.Platform.Metro
{
    public class Direct3DCoreWindow :  IFrameworkView, IDirect3DWindow
    {
        MetroGame game;
        public Direct3DCoreWindow(MetroGame game)
        {
            this.game = game;
        }

        void IFrameworkView.Initialize(CoreApplicationView applicationView)
        {
            applicationView.Activated += OnActivated;

            _device = game.Device as MetroGraphicsDevice;
            //_device = new MetroGraphicsDevice();
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

            game.Setup();
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
            _device.ClearBackBuffer(Colors.Red.ToAnkh());
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

        private MetroGraphicsDevice _device; 
        private CoreWindow _coreWindow;
    }

    public static class Direct3DCoreWindowMain 
    {
        private class FrameworkViewSource : IFrameworkViewSource
        {
            MetroGame game;
            public FrameworkViewSource(MetroGame game)
            {
                this.game = game;
            }
            IFrameworkView IFrameworkViewSource.CreateView()
            {
                MainWindow = new Direct3DCoreWindow(game);
                return MainWindow;
            }
        }

        public static void Run(MetroGame game)
        {
            var viewProviderFactory = new FrameworkViewSource(game);
            CoreApplication.Run(viewProviderFactory);
        }

        public static Direct3DCoreWindow MainWindow { get; private set; }
    }
}
