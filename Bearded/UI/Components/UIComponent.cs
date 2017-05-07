using amulware.Graphics;
using Bearded.Photones.Rendering;

namespace Bearded.Photones.UI.Components
{
    abstract class UIComponent
    {
        protected Bounds Bounds { get; }

        protected UIComponent(Bounds bounds) {
            Bounds = bounds;
        }

        public virtual void Update(UpdateEventArgs args) { }
        public virtual void HandleInput(InputState inputState) { }
        public abstract void Draw(GeometryManager geometries);
    }
}
