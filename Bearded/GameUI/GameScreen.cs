using amulware.Graphics;
using Bearded.Photones.Particles;
using Bearded.Photones.Performance;
using Bearded.Photones.Rendering;
using Bearded.Photones.Rendering.Camera;
using Bearded.Photones.Screens;
using Bearded.Photones.UI;
using OpenTK;
using GameLogic;
using Bearded.Utilities.SpaceTime;

namespace Bearded.Photones.GameUI {
    class GameScreen : ScreenLayer {
        public const float TIME_MODIFIER = 1.0f;

        private readonly Camera3D _camera;
        private readonly Beardgame _game;
        private readonly GeometryManager _geometries;

        public override Matrix4 ProjectionMatrix => _camera.Projection;
        public override Matrix4 ViewMatrix => _camera.View;

        public GameScreen(ScreenManager screenManager, GeometryManager geometryManager)
                : base(screenManager) {
            _camera = new Camera3D();
            _game = new Beardgame();
            _geometries = geometryManager;
        }

        public override void Update(Tracer tracer, BeardedUpdateEventArgs args) {
            var elapsedTime = new TimeSpan(args.UpdateEventArgs.ElapsedTimeInS * TIME_MODIFIER);
            _game.Update(tracer, elapsedTime);
            ParticleSystem.Get.Update(elapsedTime);
        }

        public override void Draw() {
            _game.Draw(_geometries);

            ParticleSystem.Get.Draw(_geometries);
        }

        public override bool HandleInput(UpdateEventArgs args, InputState inputState) {
            _camera.ChangeDistance(-inputState.InputManager.DeltaScroll * .1f);
            return true;
        }
    }
}
