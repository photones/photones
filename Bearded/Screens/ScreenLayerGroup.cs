using amulware.Graphics;
using Bearded.Photones.Performance;
using Bearded.Photones.UI;
using GameLogic;

namespace Bearded.Photones.Screens {
    abstract class ScreenLayerGroup : ScreenLayerCollection, IScreenLayer {
        private readonly ScreenLayerCollection parent;

        protected ScreenLayerGroup(ScreenLayerCollection parent) {
            this.parent = parent;
        }

        public void Update(BeardedUpdateEventArgs args) {
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