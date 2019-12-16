using Bearded.Photones.Rendering;
using GameLogic;
using OpenTK;
using TimeSpan = Bearded.Utilities.SpaceTime.TimeSpan;
using amulware.Graphics;

namespace Bearded.Photones.GameUI {

    class Beardgame {
        private readonly GameState _gameState;

        public Beardgame(GameState gameState) {
            _gameState = gameState;
        }

        public void Update(TimeSpan elapsedS) {
            _gameState.Update(elapsedS);

            // Explosions
            foreach (var gameObject in _gameState.DeadGameObjects) {
                ExplosionManager.Explode(gameObject);
            }
        }

        public void Draw(GeometryManager geometries) {
            // Coord system
            var coordcolor = Color.Red;
            var size = 0.1f;
            geometries.PhotonGeometry.DrawParticle(new Vector2(1, 1), size, coordcolor);
            geometries.PhotonGeometry.DrawParticle(new Vector2(1, -1), size, coordcolor);
            geometries.PhotonGeometry.DrawParticle(new Vector2(-1, 1), size, coordcolor);
            geometries.PhotonGeometry.DrawParticle(new Vector2(-1, -1), size, coordcolor);

            // Draw player target
            foreach (var player in _gameState.Players) {
                geometries.PhotonGeometry.DrawParticle(player.State.Target.NumericValue, size, coordcolor);
            }

            var renderer = new GameObjectRenderer(geometries);
            foreach (var gameObject in _gameState.GameObjects) {
                renderer.Render(_gameState, gameObject);
            }
        }
    }
}
