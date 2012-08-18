using System;
using System.Drawing.Drawing2D;
using Ankh;


namespace Defenetron {
    public class RenderHandler 
    {
			public GraphicsDeviceBase device;
        private int vertexBufferIndex;

				public RenderHandler(GraphicsDeviceBase d) 
        {
            device = d;

            var coords = new[] { 
                device.CreateVector(0f, 0f, 1f, 1f),
                device.CreateVector(0f, 1f, 1f, 1f),
                device.CreateVector(1f, 0f, 1f, 1f)
            };

            vertexBufferIndex = device.MakeVertexBuffer(sizeof(float) * 4 * coords.Length);
            device.FillVertexBuffer(vertexBufferIndex, coords);
        }

        public void Render() 
        {
            //device.ClearBackBuffer(device.CreateColor(123.0f / 255.0f, 160.0f / 255.0f, 183.0f / 255.0f, 1));
            //device.FillVertexBuffer(vertexBufferIndex, coords);
            device.Present();
        }
    }
}
