using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using SharpDX;
using DXGI = SharpDX.DXGI;
using D3D = SharpDX.Direct3D;
using D3D11 = SharpDX.Direct3D11;

namespace Defenetron8
{
    abstract class Direct3DBase
    {
        public virtual void Initialize(CoreWindow window)
        {
            _window = window;

            CreateDeviceResources();
            CreateWindowSizeDependentResources();
        }

        protected virtual void CreateDeviceResources()
        {
            var creationFlags = D3D11.DeviceCreationFlags.BgraSupport;
            creationFlags |= D3D11.DeviceCreationFlags.Debug;

            var device = new D3D11.Device(D3D.DriverType.Hardware, creationFlags,
                D3D.FeatureLevel.Level_11_1,
                D3D.FeatureLevel.Level_11_0,
                D3D.FeatureLevel.Level_10_1,
                D3D.FeatureLevel.Level_10_0,
                D3D.FeatureLevel.Level_9_3,
                D3D.FeatureLevel.Level_9_2,
                D3D.FeatureLevel.Level_9_1);
            _device = device.QueryInterface<D3D11.Device1>();
            _deviceContext = device.ImmediateContext.QueryInterface<D3D11.DeviceContext1>();
        }

        // Allocate all memory resources that change on a window SizeChanged event.
        protected virtual void CreateWindowSizeDependentResources()
        {
            _windowBounds = _window.Bounds;

            if (_swapChain != null)
            {
                _swapChain.ResizeBuffers(2, 0, 0, DXGI.Format.B8G8R8A8_UNorm, 0);
            }
            else
            {
                var swapChainDesc = new DXGI.SwapChainDescription1()
                    {
                        Width = 0,
                        Height = 0,
                        Format = DXGI.Format.B8G8R8A8_UNorm,
                        Stereo = false,
                        Usage = DXGI.Usage.RenderTargetOutput,
                        BufferCount = 2,
                        Scaling = DXGI.Scaling.None,
                        SwapEffect = DXGI.SwapEffect.FlipSequential,
                        SampleDescription = new DXGI.SampleDescription { Count = 1, Quality = 0 },
                        Flags = 0
                    };

                var dxgiDevice = _device.QueryInterface<DXGI.Device1>();
                var dxgiAdapter = dxgiDevice.Adapter;

                var dxgiFactory = dxgiAdapter.GetParent<DXGI.Factory2>();

                var coWindow = new SharpDX.ComObject(_window);
                _swapChain = dxgiFactory.CreateSwapChainForCoreWindow(dxgiDevice, coWindow, ref swapChainDesc, null);

                dxgiDevice.MaximumFrameLatency = 1;

                D3D11.Texture2D backBuffer = _swapChain.GetBackBuffer<D3D11.Texture2D>(0);

                _renderTargetView = new D3D11.RenderTargetView(_device, backBuffer);

                // Cache the rendertarget dimensions in our helper class for convenient use.
                _renderTargetSize.Width = backBuffer.Description.Width;
                _renderTargetSize.Height = backBuffer.Description.Height;

                // Create a descriptor for the depth/stencil buffer.
                var depthStencilDesc = new D3D11.Texture2DDescription
                    {
                        Format = DXGI.Format.D24_UNorm_S8_UInt,
                        Width = backBuffer.Description.Width,
                        Height = backBuffer.Description.Height,
                        ArraySize = 1,
                        MipLevels = 1,
                        BindFlags = D3D11.BindFlags.DepthStencil,
                        SampleDescription = new DXGI.SampleDescription { Count = 1 }
                    };

                // Allocate a 2-D surface as the depth/stencil buffer.
                var depthStencil = new D3D11.Texture2D(_device, depthStencilDesc);

                // Create a DepthStencil view on this surface to use on bind.
                _depthStencilView = new D3D11.DepthStencilView(_device, depthStencil, 
                    new D3D11.DepthStencilViewDescription { Dimension = D3D11.DepthStencilViewDimension.Texture2D });

                // Create a viewport descriptor of the full window size.
                var viewPort = new D3D11.Viewport
                    {
                        TopLeftX = 0.0f,
                        TopLeftY = 0.0f,
                        Width = backBuffer.Description.Width,
                        Height = backBuffer.Description.Height
                    };

                // Set the current viewport using the descriptor.
                _deviceContext.Rasterizer.SetViewports(viewPort);
            }
        }

        protected void ClearBackBuffer(Color4 color)
        {
            _deviceContext.OutputMerger.SetTargets(_depthStencilView, _renderTargetView);
            _deviceContext.Rasterizer.SetViewport(0, 0, (float)_renderTargetSize.Width, (float)_renderTargetSize.Height, 0, 1);
            _deviceContext.ClearRenderTargetView(_renderTargetView, color);
            _deviceContext.ClearDepthStencilView(
                _depthStencilView,
                D3D11.DepthStencilClearFlags.Depth | D3D11.DepthStencilClearFlags.Stencil, 1, 0);
        }

        protected void Present()
        {
            _swapChain.Present(1, DXGI.PresentFlags.None);
        }

        /*
         * void DirectXBase::SetDpi(float dpi)
{
    if (dpi != m_dpi)
    {
        // Save the DPI of this display in our class.
        m_dpi = dpi;
        
        // Update Direct2D's stored DPI.
        m_d2dContext->SetDpi(m_dpi, m_dpi);

        // Often a DPI change implies a window size change. In some cases Windows will issue
        // both a size changed event and a DPI changed event. When this happens, the resulting bounds 
        // will not change, and the window resize code will only be executed once.
        UpdateForWindowSizeChange();
    }
}


         * */

        private D3D11.Device1 _device;
        private D3D11.DeviceContext1 _deviceContext;

        private D3D11.RenderTargetView _renderTargetView;
        private Windows.Foundation.Rect _renderTargetSize;

        private D3D11.DepthStencilView _depthStencilView;
        
        private CoreWindow _window;
        private Windows.Foundation.Rect _windowBounds;

        private DXGI.SwapChain1 _swapChain;
    }
}
