using Bearded.Photones.Performance;
using Bearded.Photones.Rendering;
using Bearded.Photones.Screens;
using OpenTK;

namespace Bearded.Photones.GameUI {
    class HudScreen : UIScreenLayer {
        private readonly GeometryManager _geometries;
        private PerformanceSummary _stats;

        public HudScreen(ScreenLayerCollection parent, GeometryManager geometries) : base(parent, geometries) {
            _geometries = geometries;
        }

        public override void Draw() {
            base.Draw();
            _geometries.ConsolasFont.Height = 20;
            _geometries.ConsolasFont.DrawString(new Vector2(0, 0), _stats.FrameTimeString);
            _geometries.ConsolasFont.DrawString(new Vector2(0, 20), _stats.ElapsedTimeString);
        }

        public override void Update(Utils.Tracer tracer, BeardedUpdateEventArgs args) {
            _stats = args.PerformanceStats;
        }
    }
}
