using System;
using Bearded.Photones.Rendering;
using GameLogic;
using OpenTK;
using TimeSpan = Bearded.Utilities.SpaceTime.TimeSpan;

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
            foreach (var photon in _gameState.Photons) {
                geometries.PhotonGeometry.DrawParticle(photon.Position.NumericValue);
            }
        }
    }
}
