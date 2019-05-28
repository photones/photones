using amulware.Graphics;
using Bearded.Photones.Rendering;
using Bearded.Photones.UI;

namespace Bearded.Photones.Screens {
    interface IScreenLayer {
        void Update(UpdateEventArgs args);
        // Should return false if the input should not be propagated.
        bool HandleInput(UpdateEventArgs args, InputState inputState);
        void OnResize(ViewportSize newSize);
        void Render(RenderContext context);
    }
}