using amulware.Graphics;
using Bearded.Photones.UI;

namespace Bearded.Photones.Screens {
    abstract class ScreenLayerGroup : ScreenLayerCollection, IScreenLayer {
        private readonly ScreenLayerCollection parent;

        protected ScreenLayerGroup(ScreenLayerCollection parent) {
            this.parent = parent;
        }

        public void Update(UpdateEventArgs args) {
            UpdateAll(args);
        }

        public bool HandleInput(UpdateEventArgs args, InputState inputState) {
            return PropagateInput(args, inputState);
        }

        protected void Destroy() {
            parent.RemoveScreenLayer(this);
        }
    }
}