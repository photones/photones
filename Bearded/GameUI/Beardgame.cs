using System;
using Bearded.Photones.Rendering;
using GameLogic;
using OpenTK;
using TimeSpan = Bearded.Utilities.SpaceTime.TimeSpan;

namespace Bearded.Photones.GameUI
{
    class Beardgame
    {
        const float PHOTON_SIZE = 0.01f;

        private GameState _gameState;

        public Beardgame() {
            _gameState = GameStateFactory.BuildInitialGameState();
        }

        public void Update(TimeSpan elapsedTime) {
            _gameState = _gameState.Update(elapsedTime);
        }

        public void Draw(GeometryManager geometries) {
            var fooGeo = geometries.FooGeometry;
            foreach (var photon in _gameState.Photons) {
                fooGeo.Size = new Vector2(PHOTON_SIZE);
                fooGeo.DrawSprite(photon.Position.NumericValue);
            }
        }
    }
}
