using System;

namespace Defenetron {
    public class Game {
        private RenderHandler _render;

        public Game(GraphicsDevice renderengine) {
            _render = new RenderHandler(renderengine);
            this.setup();
        }

        public void render() {
            _render.render();
        }

        // to implement
        public virtual void setup() { }
    }
}
