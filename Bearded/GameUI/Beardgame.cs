using System;
using Bearded.Photones.Rendering;
using GameLogic;
using OpenTK;
using TimeSpan = Bearded.Utilities.SpaceTime.TimeSpan;

namespace Bearded.Photones.GameUI
{
    class Beardgame
    {
        const float PLANET_SIZE = 0.25f;
        const float PHOTON_SIZE = 0.01f;

        private GameState _gameState;

        public Beardgame() {
            _gameState = GameLogic.GameStateFactory.BuildInitialGameState();
        }

        public void Update(TimeSpan elapsedTime) {
            _gameState = _gameState.Update(elapsedTime);
        }

        public void Draw(GeometryManager geometries) {
            var particleGeo = geometries.ParticleGeometry;
            foreach (var planet in _gameState.Planets) {
                particleGeo.Size = new Vector2(PLANET_SIZE);
                particleGeo.DrawSprite(planet.Position.NumericValue);
            }

            foreach (var photon in _gameState.Photons) {
                particleGeo.Size = new Vector2(PHOTON_SIZE);
                particleGeo.DrawSprite(photon.Position.NumericValue);
            }
        }
    }
}
