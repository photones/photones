using Bearded.Photones.Performance;
using Bearded.Photones.Rendering;
using Bearded.Photones.Screens;
using OpenTK;
using GameLogic;

namespace Bearded.Photones.GameUI {
    class HudScreen : UIScreenLayer {
        private readonly GeometryManager _geometries;
        private PerformanceSummary _stats;
        private readonly GameState _gameState;

        public HudScreen(ScreenLayerCollection parent, GeometryManager geometries, GameState gameState)
            : base(parent, geometries) {
            _geometries = geometries;
            _gameState = gameState;
        }

        public override void Draw() {
            base.Draw();
            _geometries.ConsolasFont.Height = 20;
            _geometries.ConsolasFont.DrawString(new Vector2(0, 0), _stats.FrameTimeString);
            _geometries.ConsolasFont.DrawString(new Vector2(0, 20), _stats.ElapsedTimeString);
            _geometries.ConsolasFont.DrawString(new Vector2(0, 40), _stats.NrGameObjectsString);

            var timeMod = $"Time Mod: {_gameState.GameParameters.TimeModifier:0.000}";
            _geometries.ConsolasFont.DrawString(new Vector2(0, 60), timeMod);
            var modA = $"Mod A: {_gameState.GameParameters.ModA:0.000}";
            _geometries.ConsolasFont.DrawString(new Vector2(0, 80), modA);
            var modB = $"Mod B: {_gameState.GameParameters.ModB:0.000}";
            _geometries.ConsolasFont.DrawString(new Vector2(0, 100), modB);
            var modC = $"Mod C: {_gameState.GameParameters.ModC:0.000}";
            _geometries.ConsolasFont.DrawString(new Vector2(0, 120), modC);
            var modD = $"Int Mod D: {_gameState.GameParameters.IntModD:0}";
            _geometries.ConsolasFont.DrawString(new Vector2(0, 140), modD);
        }

        public override void Update(BeardedUpdateEventArgs args) {
            _stats = args.PerformanceStats;
        }
    }
}
