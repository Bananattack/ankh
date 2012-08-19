using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using SharpDX;
using SharpDX.Direct3D9;

namespace Ankh
{
	public class DX9GraphicsDevice : GraphicsDeviceBase
	{
		public Direct3D d3d;
		public Device Device;

		public class MyTexture : ITexture
		{
			DX9GraphicsDevice dev;
			public Texture tex;
			public MyTexture(DX9GraphicsDevice dev, int width, int height)
			{
				this.dev = dev;
				tex = new Texture(dev.Device, width, height, 1, Usage.Dynamic, Format.A8R8G8B8, Pool.Default);
			}
			public void WriteAll(int[] data)
			{
				var dr = tex.LockRectangle(0, LockFlags.Discard);
				System.Runtime.InteropServices.Marshal.Copy(data, 0, dr.DataPointer, data.Length);
				tex.UnlockRectangle(0);
			}
		}

		public override ITexture CreateTexture(int width, int height)
		{
			return new MyTexture(this, width, height);
		}

		public static bool IsAvailable
		{
			get
			{
				try
				{
					var temp = new Direct3D();
					temp.Dispose();
					return true;
				}
				catch
				{
					return false;
				}
			}
		}

		public DX9GraphicsDevice(Form form)
		{
			d3d = new Direct3D();

			var pp = new PresentParameters
			{
				BackBufferWidth = Math.Max(1, form.ClientSize.Width),
				BackBufferHeight = Math.Max(1, form.ClientSize.Height),
				DeviceWindowHandle = form.Handle,
				PresentationInterval = PresentInterval.Immediate,
				Windowed = true,
				AutoDepthStencilFormat = Format.D24X8,
				BackBufferCount = 1,
				BackBufferFormat = Format.X8R8G8B8,
				EnableAutoDepthStencil = true,
				SwapEffect = SwapEffect.Discard
			};

			var flags = CreateFlags.SoftwareVertexProcessing;
			if ((d3d.GetDeviceCaps(0, DeviceType.Hardware).DeviceCaps & DeviceCaps.HWTransformAndLight) != 0)
			{
				flags = CreateFlags.HardwareVertexProcessing;
			}

			Device = new Device(d3d, 0, DeviceType.Hardware, form.Handle, flags, pp);
		}
		public override void ResetDevice()
		{
		}
		public override void ClearBackBuffer(Color4 color)
		{
			Device.Clear(ClearFlags.Target, color, 1.0f, 0);
		}

		Dictionary<int, VertexBuffer> VertexBuffers = new Dictionary<int, VertexBuffer>();
		int NextVertexBufferId = 0;
		public override int MakeVertexBuffer(int size)
		{
			var vb = new VertexBuffer(Device, size, Usage.None, VertexFormat.Position, Pool.Default);
			while (VertexBuffers.ContainsKey(NextVertexBufferId))
				NextVertexBufferId++;
			VertexBuffers[NextVertexBufferId] = vb;
			return NextVertexBufferId++;
		}
		public override void FillVertexBuffer(int index, Vector4[] data)
		{
			var vb = VertexBuffers[index];
			var ds = vb.Lock(0, 0, LockFlags.Discard);
			ds.WriteRange(data);
			vb.Unlock();
		}
		public override void Present()
		{
			Device.Present();
		}
	}

}
