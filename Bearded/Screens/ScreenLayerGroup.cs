using amulware.Graphics;
using Bearded.Photones.Performance;
using Bearded.Photones.UI;

namespace Bearded.Photones.Screens {
    abstract class ScreenLayerGroup : ScreenLayerCollection, IScreenLayer {
        private readonly ScreenLayerCollection parent;

        protected ScreenLayerGroup(ScreenLayerCollection parent) {
            this.parent = parent;
        }

        public void Update(Utils.Tracer tracer, BeardedUpdateEventArgs args) {
            UpdateAll(tracer, args);
        }

        public bool HandleInput(UpdateEventArgs args, InputState inputState) {
            return PropagateInput(args, inputState);
        }

        protected void Destroy() {
            parent.RemoveScreenLayer(this);
        }
    }
}