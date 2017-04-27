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

namespace Bearded.Photones.Game.UI
{
    class GameScreen : ScreenLayer
    {
        private readonly Camera2D camera;
        private readonly Game game;
        private readonly GeometryManager geometries;

        public override Matrix4 ProjectionMatrix => camera.Projection;
        public override Matrix4 ViewMatrix => camera.View;

        public GameScreen(ScreenManager screenManager, GeometryManager geometryManger)
                : base(screenManager) {
            camera = new Camera2D();
            game = new Game();
            geometries = geometryManger;
        }

        public override void Update(UpdateEventArgs args) {
            var elapsedTime = new Bearded.Utilities.SpaceTime.TimeSpan(args.ElapsedTimeInS);

            game.Update(elapsedTime);

            ParticleSystem.Get.Update(elapsedTime);
        }

        public override void Draw() {
            game.Draw(geometries);

            ParticleSystem.Get.Draw(geometries);
        }

        public override bool HandleInput(UpdateEventArgs args, InputState inputState) {
            return true;
        }
    }
}
