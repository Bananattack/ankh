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
using Buffer = SharpDX.Direct3D11.Buffer;

namespace Ankh.Platform.Win32.DX11
{
	public class DX11GraphicsDevice : Win32GraphicsDevice
	{
		public static void SafeDispose(DisposeBase disposable)
		{
			if (disposable != null && !disposable.IsDisposed)
			{
				disposable.Dispose();
			}
		}

		public static bool IsAvailable
		{
			get
			{
				try
				{
					var temp = new Device(DriverType.Null);
					temp.Dispose();
					return true;
				}
				catch
				{
					return false;
				}
			}
		}

		private const Format format = Format.R8G8B8A8_UNorm;

		private Device device;
		private DeviceContext context;
		private SwapChain swapChain;

		private RenderTargetView renderTargetView;
		private DepthStencilView depthStencilView;
		private Texture2D depthBuffer;

		private int lastVertexBuffer = -1;
		private Buffer[] vertexBuffers = new Buffer[32];
		private DataBox[] vertexBufferDataBoxes = new DataBox[32];

		class MyTexture : ITexture
		{
			DX11GraphicsDevice dev;
			Texture2D tex;
			public MyTexture(DX11GraphicsDevice dev, int width, int height)
			{
				this.dev = dev;
				var descr = new Texture2DDescription
					{
                        Format = Format.B8G8R8A8_UNorm,
                        Width = width,
                        Height = height,
                        MipLevels = 1,
                        ArraySize = 1,
                        BindFlags = BindFlags.ShaderResource,
                        SampleDescription = new SampleDescription { Count = 1 }
					};
				tex = new Texture2D(dev.device, descr);
			}

			public void WriteAll(int[] data)
			{
				dev.context.UpdateSubresource(data, tex);
			}
		}

		public override ITexture CreateTexture(int width, int height)
		{
			return new MyTexture(this, width, height);
		}

		public DX11GraphicsDevice(Form form)
		{
			this.Form = form;

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

			var levels = new[] { FeatureLevel.Level_9_2, FeatureLevel.Level_9_1 };

			Device.CreateWithSwapChain(
					DriverType.Hardware,
					DeviceCreationFlags.None,
					levels,
					scDesc,
					out device,
					out swapChain);

			ResetDevice();

			form.ResizeEnd += ResizeEnd;
		}



		void ResizeEnd(object sender, EventArgs e)
		{
			ResetDevice();
		}

		public override void ResetDevice()
		{
			SafeDispose(renderTargetView);
			SafeDispose(depthStencilView);
			SafeDispose(depthBuffer);

			swapChain.ResizeBuffers(2, Form.ClientSize.Width, Form.ClientSize.Height, format, SwapChainFlags.AllowModeSwitch);

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
			depthDesc.Width = Form.ClientSize.Width;
			depthDesc.Height = Form.ClientSize.Height;

			depthBuffer = new Texture2D(device, depthDesc);
			depthStencilView = new DepthStencilView(device, depthBuffer);

			context = device.ImmediateContext;
		}

		public override void ClearBackBuffer(Color4 color)
		{
			context.OutputMerger.SetTargets(depthStencilView, renderTargetView);
			context.Rasterizer.SetViewport(0, 0, Form.ClientSize.Width, Form.ClientSize.Height, 0, 1);
			context.ClearRenderTargetView(renderTargetView, color.ToSharpDX());
			context.ClearDepthStencilView(
					depthStencilView,
					DepthStencilClearFlags.Depth | DepthStencilClearFlags.Stencil, 1, 0);
		}

		public override int MakeVertexBuffer(int size)
		{
			var bufferDesc = new BufferDescription(
					size,
					SharpDX.Direct3D11.ResourceUsage.Dynamic,
					SharpDX.Direct3D11.BindFlags.VertexBuffer,
					SharpDX.Direct3D11.CpuAccessFlags.Write,
					0,
					3
			);
			var buffer = new Buffer(device, bufferDesc);
			var bufferBinding = new VertexBufferBinding(buffer, 0, 0);
			var databox = new DataBox();

			// TODO there are no bounds checking here; we can only actually add 16 or 32
			lastVertexBuffer++;

			vertexBuffers[lastVertexBuffer] = buffer;
			vertexBufferDataBoxes[lastVertexBuffer] = databox;
			context.InputAssembler.SetVertexBuffers(lastVertexBuffer, bufferBinding);

			return lastVertexBuffer;
		}

		public override void FillVertexBuffer(int index, Vector4[] data)
		{
			context.UpdateSubresource<Vector4>(data, vertexBuffers[index]);
		}

		public override void Present()
		{
			swapChain.Present(1, PresentFlags.None);
		}

        class SpriteBatch : ISpriteBatch
        {
            DX11GraphicsDevice dev;
            public SpriteBatch(DX11GraphicsDevice dev)
            {
                this.dev = dev;
            }

            public void Dispose()
            {
            }

            public void Draw(ITexture tex, Vector3 position)
            {
                //FIXME
                //seriously? no spritebatch in dx11? augh
            }
        }

		public override ISpriteBatch CreateSpriteBatch()
		{
            return new SpriteBatch(this);
		}
	}
}
