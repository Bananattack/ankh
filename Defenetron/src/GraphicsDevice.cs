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

        private Form form;

        private const Format format = Format.R8G8B8A8_UNorm;

        private Device device;
        private SwapChain swapChain;

        private RenderTargetView renderTargetView;
        private DepthStencilView depthStencilView;
        private Texture2D depthBuffer;

        public void CreateDevice(Form form)
        {
            this.form = form;
            var scDesc = new SwapChainDescription
                             {
                                 BufferCount = 2,
                                 Flags = SwapChainFlags.AllowModeSwitch,
                                 IsWindowed = true,
                                 ModeDescription = new ModeDescription(
                                     form.ClientSize.Width,
                                     form.ClientSize.Height,
                                     new Rational(60, 1),
                                     format),
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
                out device,
                out swapChain);

            ResetDevice();

            form.ResizeEnd += _form_ResizeEnd;
        }

        void _form_ResizeEnd(object sender, EventArgs e)
        {
            ResetDevice();
        }
        
        public void ResetDevice()
        {
            SafeDispose(renderTargetView);
            SafeDispose(depthStencilView);
            SafeDispose(depthBuffer);

            swapChain.ResizeBuffers(2, form.ClientSize.Width, form.ClientSize.Height, format, SwapChainFlags.AllowModeSwitch);

            using (var resource = Resource.FromSwapChain<Texture2D>(swapChain, 0))
            {
                renderTargetView = new RenderTargetView(device, resource);
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
            depthDesc.Width = form.ClientSize.Width;
            depthDesc.Height = form.ClientSize.Height;

            depthBuffer = new Texture2D(device, depthDesc);
            depthStencilView = new DepthStencilView(device, depthBuffer);
        }

        public void ClearBackBuffer(Color4 color)
        {
            var context = device.ImmediateContext;
            context.OutputMerger.SetTargets(depthStencilView, renderTargetView);
            context.Rasterizer.SetViewport(0, 0, form.ClientSize.Width, form.ClientSize.Height, 0, 1);
            context.ClearRenderTargetView(renderTargetView, color);
            context.ClearDepthStencilView(
                depthStencilView,
                DepthStencilClearFlags.Depth | DepthStencilClearFlags.Stencil, 1, 0);
        }

        public Color4 CreateColor(float r, float g, float b, float a = 1.0f) {
            return new Color4(r, g, b, a);
        }

        public void Present()
        {
            swapChain.Present(1, PresentFlags.None);
        }
    }
}
