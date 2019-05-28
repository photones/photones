using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using amulware.Graphics;
using Bearded.Photones.Particles;
using Bearded.Photones.Rendering;
using Bearded.Photones.Rendering.Camera;
using Bearded.Photones.Screens;
using Bearded.Photones.UI;
using Bearded.Utilities.SpaceTime;
using OpenTK;
using OpenTK.Input;

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

        public override void Update(UpdateEventArgs args) {
            var elapsedTime = new Bearded.Utilities.SpaceTime.TimeSpan(args.ElapsedTimeInS);

            _game.Update(elapsedTime);

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
