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
	public class GraphicsDeviceFactory
	{

		/// <summary>
		/// attempts to create a DirectX-type device, preferring 11 if available and trying 9 otherwise
		/// </summary>
		public static GraphicsDeviceBase CreateDxDevice()
		{
			try
			{
				var dx11 = new DX11GraphicsDevice();
				if (dx11.IsAvailable) return dx11;
			}
			catch
			{
			}

			try
			{
				var dx9 = new DX9GraphicsDevice();
				if (dx9.IsAvailable) return dx9;
			}
			catch
			{
			}

			return null;
		}
	}

	public abstract class GraphicsDeviceBase
	{
		public abstract void CreateDevice(Form form);
		public abstract void ResetDevice();
		public abstract void ClearBackBuffer(Color4 color);

		//dont think these are great ideas
		public abstract int MakeVertexBuffer(int size);
		public abstract void FillVertexBuffer(int index, Vector4[] data);

		public abstract void Present();

		public abstract bool IsAvailable { get; }

		//do we really need these? i dont think i want to view them as device resources -zero
		public Color4 CreateColor(float r, float g, float b, float a = 1.0f)
		{
			return new Color4(r, g, b, a);
		}

		//do we really need these? i dont think i want to view them as device resources -zero
		public Vector4 CreateVector(float x, float y, float z, float w)
		{
			return new Vector4(x, y, z, w);
		}
	}

	public class DX9GraphicsDevice : GraphicsDeviceBase
	{
		private Direct3D d3d;
		private Device Device;

		public override bool IsAvailable
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

		public override void CreateDevice(Form form)
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
			Device.Clear(ClearFlags.Target,color,1.0f,0);
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
