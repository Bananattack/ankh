using System;

namespace Defenetron {
    public class Game {
        private RenderHandler RenderEngine;

        public Game(GraphicsDevice device) {
            RenderEngine = new RenderHandler(device);
            this.Setup();
        }

        public void Render() {
            RenderEngine.Render();
        }

        // to implement
        public virtual void Setup() { }
    }
}

