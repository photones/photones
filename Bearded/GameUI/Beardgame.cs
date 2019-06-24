using Bearded.Photones.Rendering;
using GameLogic;
using OpenTK;
using TimeSpan = Bearded.Utilities.SpaceTime.TimeSpan;
using amulware.Graphics;

namespace Bearded.Photones.GameUI {

    class Beardgame {
        private GameState _gameState;

        public Beardgame() {
            _gameState = GameStateFactory.BuildInitialGameState();
        }

        public void Update(TimeSpan elapsedTime) {
            _gameState = _gameState.Update(elapsedTime);
        }

        public void Draw(GeometryManager geometries) {
            // Coord system
            var coordcolor = Color.Red;
            geometries.PhotonGeometry.DrawParticle(new Vector2(0, 0), coordcolor);
            geometries.PhotonGeometry.DrawParticle(new Vector2(1, 1), coordcolor);
            geometries.PhotonGeometry.DrawParticle(new Vector2(1, -1), coordcolor);
            geometries.PhotonGeometry.DrawParticle(new Vector2(-1, 1), coordcolor);
            geometries.PhotonGeometry.DrawParticle(new Vector2(-1, -1), coordcolor);

            // Photons
            var photoncolor = Color.DarkGoldenrod;
            foreach (var photon in _gameState.Photons) {
                geometries.PhotonGeometry.DrawParticle(photon.Position.NumericValue, photoncolor);
            }
        }
    }
}
