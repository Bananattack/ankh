using System;
using System.Drawing.Drawing2D;

namespace Defenetron {
    public class RenderHandler {
        private GraphicsDevice _backend;

        public RenderHandler(GraphicsDevice backend) {
            _backend = backend;
        }

        public void Render() {
            _backend.ClearBackBuffer(_backend.CreateColor(123.0f / 255.0f, 160.0f / 255.0f, 183.0f / 255.0f, 1));

            _backend.Present();
        }
    }
}
