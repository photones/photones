using Bearded.Photones.Rendering;
using GameLogic;
using OpenTK;
using System;
using TimeSpan = Bearded.Utilities.SpaceTime.TimeSpan;

namespace Bearded.Photones.GameUI {
    class Beardgame {
        private GameState _gameState;
        private double _fpsSmoothAvg = 60;
        private double _fpsSmoothDev = 10;

        public Beardgame() {
            _gameState = GameStateFactory.BuildInitialGameState();
        }

        public void Update(TimeSpan elapsedTime) {
            if (elapsedTime > new TimeSpan(0.0001)) // This is basically to skip the first frame
            {
                var fps = TimeSpan.One / elapsedTime;
                var a = 0.05;
                _fpsSmoothAvg = a * fps + (1 - a) * _fpsSmoothAvg;
                var dev = Math.Abs(_fpsSmoothAvg - fps);
                var b = 0.1;
                _fpsSmoothDev = b * dev + (1 - b) * _fpsSmoothDev;
            }
            _gameState = _gameState.Update(elapsedTime);
        }

        public void Draw(GeometryManager geometries) {
            geometries.ConsolasFont.Height = 0.1f;
            geometries.ConsolasFont.SizeCoefficient = new Vector2(1, -1);
            geometries.ConsolasFont.DrawString(new Vector2(-1, 1), $"{(int)_fpsSmoothAvg} +/- {(int)_fpsSmoothDev}");
            foreach (var photon in _gameState.Photons) {
                geometries.PhotonGeometry.DrawParticle(photon.Position.NumericValue);
            }
        }
    }
}
