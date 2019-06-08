using Bearded.Photones.Rendering;
using Bearded.Photones.Screens;
using OpenTK;

namespace Bearded.Photones.GameUI {
    class HudScreen : UIScreenLayer {
        private readonly GeometryManager _geometries;

        public HudScreen(ScreenLayerCollection parent, GeometryManager geometries) : base(parent, geometries) {
            _geometries = geometries;
        }

        public override void Draw() {
            _geometries.ConsolasFont.Height = 20;
            _geometries.ConsolasFont.DrawString(new Vector2(0, 0), "Top left");
            _geometries.ConsolasFont.DrawString(new Vector2(ViewportSize.Width, ViewportSize.Height), "Bottom right", 1, 1);
        }
    }
}
