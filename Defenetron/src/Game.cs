using System;

namespace Defenetron {
    public class Game {
        private RenderHandler renderEngine;

        public Game(GraphicsDevice device) {
            renderEngine = new RenderHandler(device);
            this.Setup();
        }

        public void Render() {
            renderEngine.Render();
        }

        // to implement
        public virtual void Setup() { }
    }
}

