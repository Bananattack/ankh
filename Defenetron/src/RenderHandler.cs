using System;
using System.Drawing.Drawing2D;

namespace Defenetron {
    public class RenderHandler 
    {
        private GraphicsDevice device;

        public RenderHandler(GraphicsDevice d) 
        {
            device = d;
        }

        public void Render() 
        {
            device.ClearBackBuffer(device.CreateColor(123.0f / 255.0f, 160.0f / 255.0f, 183.0f / 255.0f, 1));

            device.Present();
        }
    }
}
