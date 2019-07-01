﻿using amulware.Graphics;
using Bearded.Photones.Particles;
using Bearded.Photones.Performance;
using Bearded.Photones.Rendering;
using Bearded.Photones.Rendering.Camera;
using Bearded.Photones.Screens;
using Bearded.Photones.UI;
using OpenTK;

namespace Bearded.Photones.GameUI {
    class GameScreen : ScreenLayer {
        private readonly Camera3D _camera;
        private readonly Beardgame _game;
        private readonly GeometryManager _geometries;

        public override Matrix4 ProjectionMatrix => _camera.Projection;
        public override Matrix4 ViewMatrix => _camera.View;

        public GameScreen(ScreenManager screenManager, GeometryManager geometryManger)
                : base(screenManager) {
            _camera = new Camera3D();
            _game = new Beardgame();
            _geometries = geometryManger;
        }

        public override void Update(BeardedUpdateEventArgs args) {
            var elapsedTime = new Bearded.Utilities.SpaceTime.TimeSpan(args.UpdateEventArgs.ElapsedTimeInS);
            var totalTime = new Bearded.Utilities.SpaceTime.TimeSpan(args.UpdateEventArgs.TimeInS);

            _game.Update(elapsedTime, totalTime);

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
