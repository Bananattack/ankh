using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using Device = SharpDX.Direct3D11.Device;
using Resource = SharpDX.Direct3D11.Resource;

namespace Defenetron
{
    public class GraphicsDevice
    {
        public static void SafeDispose(DisposeBase disposable)
        {
            if(disposable !=  null && !disposable.IsDisposed)
            {
                disposable.Dispose();
            }
        }

        private Form _form;

        private const Format _format = Format.R8G8B8A8_UNorm;

        private Device _device;
        private SwapChain _swapChain;

        private RenderTargetView _renderTargetView;
        private DepthStencilView _depthStencilView;
        private Texture2D _depthBuffer;

        public void CreateDevice(Form form)
        {
            _form = form;
            var scDesc = new SwapChainDescription
                             {
                                 BufferCount = 2,
                                 Flags = SwapChainFlags.AllowModeSwitch,
                                 IsWindowed = true,
                                 ModeDescription = new ModeDescription(
                                     form.ClientSize.Width,
                                     form.ClientSize.Height,
                                     new Rational(60, 1),
                                     _format),
                                 OutputHandle = form.Handle,
                                 SampleDescription = new SampleDescription(1, 0),
                                 SwapEffect = SwapEffect.Sequential,
                                 Usage = Usage.RenderTargetOutput
                             };

            var levels = new[] {FeatureLevel.Level_9_2, FeatureLevel.Level_9_1};

            Device.CreateWithSwapChain(
                DriverType.Hardware,
                DeviceCreationFlags.None,
                levels,
                scDesc,
                out _device,
                out _swapChain);

            ResetDevice();

            _form.ResizeEnd += _form_ResizeEnd;
        }

        void _form_ResizeEnd(object sender, EventArgs e)
        {
            ResetDevice();
        }
        
        public void ResetDevice()
        {
            SafeDispose(_renderTargetView);
            SafeDispose(_depthStencilView);
            SafeDispose(_depthBuffer);

            _swapChain.ResizeBuffers(2, _form.ClientSize.Width, _form.ClientSize.Height, _format, SwapChainFlags.AllowModeSwitch);

            using (var resource = Resource.FromSwapChain<Texture2D>(_swapChain, 0))
            {
                _renderTargetView = new RenderTargetView(_device, resource);
            }

            var depthDesc = new Texture2DDescription();
            depthDesc.ArraySize = 1;
            depthDesc.BindFlags = BindFlags.DepthStencil;
            depthDesc.CpuAccessFlags = CpuAccessFlags.None;
            depthDesc.Format = Format.D24_UNorm_S8_UInt;
            depthDesc.MipLevels = 1;
            depthDesc.OptionFlags = ResourceOptionFlags.None;
            depthDesc.SampleDescription = new SampleDescription(1, 0);
            depthDesc.Usage = ResourceUsage.Default;
            depthDesc.Width = _form.ClientSize.Width;
            depthDesc.Height = _form.ClientSize.Height;

            _depthBuffer = new Texture2D(_device, depthDesc);
            _depthStencilView = new DepthStencilView(_device, _depthBuffer);
        }

        public void ClearBackBuffer(Color4 color)
        {
            var context = _device.ImmediateContext;
            context.OutputMerger.SetTargets(_depthStencilView, _renderTargetView);
            context.Rasterizer.SetViewport(0, 0, _form.ClientSize.Width, _form.ClientSize.Height, 0, 1);
            context.ClearRenderTargetView(_renderTargetView, color);
            context.ClearDepthStencilView(
                _depthStencilView,
                DepthStencilClearFlags.Depth | DepthStencilClearFlags.Stencil, 1, 0);
        }

        public Color4 CreateColor(float r, float g, float b, float a = 1.0f) {
            return new Color4(r, g, b, a);
        }

        public void Present()
        {
            _swapChain.Present(1, PresentFlags.None);
        }
    }
}
