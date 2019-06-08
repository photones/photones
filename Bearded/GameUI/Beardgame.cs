using Bearded.Photones.Rendering;
using GameLogic;
using OpenTK;
using TimeSpan = Bearded.Utilities.SpaceTime.TimeSpan;
using amulware.Graphics;

namespace Bearded.Photones.GameUI {

    class Beardgame {
        private GameState _gameState;
        private FpsMeasurer _measurer;


        public Beardgame() {
            _gameState = GameStateFactory.BuildInitialGameState();
            _measurer = new FpsMeasurer();
        }

        public void Update(TimeSpan elapsedTime) {
            _measurer.StartFrame(elapsedTime);
            _gameState = _gameState.Update(elapsedTime);
            _measurer.EndFrame();
        }

        public void Draw(GeometryManager geometries) {
            // Coord system
            var coordcolor = Color.Red;
            geometries.PhotonGeometry.DrawParticle(new Vector2(0, 0), coordcolor);
            geometries.PhotonGeometry.DrawParticle(new Vector2(1, 1), coordcolor);
            geometries.PhotonGeometry.DrawParticle(new Vector2(1, -1), coordcolor);
            geometries.PhotonGeometry.DrawParticle(new Vector2(-1, 1), coordcolor);
            geometries.PhotonGeometry.DrawParticle(new Vector2(-1, -1), coordcolor);

            // FPS
            geometries.ConsolasFont.Height = 0.1f;
            geometries.ConsolasFont.SizeCoefficient = new Vector2(1, -1);
            geometries.ConsolasFont.DrawString(new Vector2(-1, 1), _measurer.FpsString + " " + _measurer.FrameTimeString);

            // Photons
            foreach (var photon in _gameState.Photons) {
                geometries.PhotonGeometry.DrawParticle(photon.Position.NumericValue);
            }
        }
    }
}
