using Bearded.Photones.Rendering;
using GameLogic;
using OpenTK;
using System;
using TimeSpan = Bearded.Utilities.SpaceTime.TimeSpan;
using amulware.Graphics;

namespace Bearded.Photones.GameUI {
    class Beardgame {
        private GameState _gameState;
        private double _fpsSmoothedAvg = 60;
        private double _fpsSmoothedDev = 10;
        private double _fpsCurrentMax = 0;
        private double _fpsCurrentMin = 100;
        private double _fpsPreviousMax = 0;
        private double _fpsPreviousMin = 100;
        private long _frameCounter = 0;

        public Beardgame() {
            _gameState = GameStateFactory.BuildInitialGameState();
        }

        public void Update(TimeSpan elapsedTime) {
            if (elapsedTime > new TimeSpan(0.0001)) { // This is basically to skip the first frame
                var fps = TimeSpan.One / elapsedTime;
                var a = 0.05;
                _fpsSmoothedAvg = a * fps + (1 - a) * _fpsSmoothedAvg;
                var dev = Math.Abs(_fpsSmoothedAvg - fps);
                var b = 0.1;
                _fpsSmoothedDev = b * dev + (1 - b) * _fpsSmoothedDev;

                _fpsCurrentMax = Math.Max(fps, _fpsCurrentMax);
                _fpsCurrentMin = Math.Min(fps, _fpsCurrentMin);

                _frameCounter++;
                if (_frameCounter > 60) {
                    _frameCounter = 0;
                    _fpsPreviousMax = _fpsCurrentMax;
                    _fpsPreviousMin = _fpsCurrentMin;
                    _fpsCurrentMax = 0;
                    _fpsCurrentMin = double.MaxValue;
                }
            }
            _gameState = _gameState.Update(elapsedTime);
        }

        public void Draw(GeometryManager geometries) {
            // Coord system
            var coordcolor = Color.Red;
            geometries.PhotonGeometry.DrawParticle(new Vector2(0,0), coordcolor);
            geometries.PhotonGeometry.DrawParticle(new Vector2(1,1), coordcolor);
            geometries.PhotonGeometry.DrawParticle(new Vector2(1,-1), coordcolor);
            geometries.PhotonGeometry.DrawParticle(new Vector2(-1,1), coordcolor);
            geometries.PhotonGeometry.DrawParticle(new Vector2(-1,-1), coordcolor);

            // FPS
            geometries.ConsolasFont.Height = 0.1f;
            geometries.ConsolasFont.SizeCoefficient = new Vector2(1, -1);
            geometries.ConsolasFont.DrawString(new Vector2(-1, 1), $"{(int)_fpsSmoothedAvg}+/-{(int)_fpsSmoothedDev} [{(int)_fpsPreviousMin},{(int)_fpsPreviousMax}]");

            // Photons
            foreach (var photon in _gameState.Photons) {
                geometries.PhotonGeometry.DrawParticle(photon.Position.NumericValue);
            }
        }
    }
}
