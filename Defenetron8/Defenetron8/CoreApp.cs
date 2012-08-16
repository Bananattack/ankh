using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Graphics.Display;
using Windows.UI.Core;
using Windows.Foundation;
using SharpDX;

namespace Defenetron8
{
    class CoreWindowEvents : Direct3DBase, IFrameworkView
    {
        void IFrameworkView.Initialize(CoreApplicationView applicationView)
        {
            applicationView.Activated += OnActivated;
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

            ((Direct3DBase)this).Initialize(window);
        }

        void IFrameworkView.Load(string entryPoint)
        {
        }

        void IFrameworkView.Run()
        {
            CoreWindow.GetForCurrentThread().Activate();

            Render();
            Present();
            CoreWindow.GetForCurrentThread().Dispatcher.ProcessEvents(CoreProcessEventsOption.ProcessUntilQuit);
        }

        private void Render()
        {
            base.ClearBackBuffer(new Color4(123.0f / 255.0f, 160.0f / 255.0f, 183.0f / 255.0f, 1));
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
            Present();
            // handle window size change
        }
    }

    class DirectXAppSource : IFrameworkViewSource
    {
        IFrameworkView IFrameworkViewSource.CreateView()
        {
            return new CoreWindowEvents();
        }
    }

    class CoreApp
    {
        public static void Main(string[] args)
        {
            var viewProviderFactory = new DirectXAppSource();
            CoreApplication.Run(viewProviderFactory);
        }
    }
}
