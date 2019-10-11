using Bearded.Photones.Rendering;
using GameLogic;
using OpenTK;
using TimeSpan = Bearded.Utilities.SpaceTime.TimeSpan;
using amulware.Graphics;

namespace Bearded.Photones.GameUI {

    class Beardgame {
        private readonly GameState _gameState;

        public Beardgame() {
            _gameState = GameStateFactory.BuildInitialGameState();
        }

        public void Update(Tracer tracer, TimeSpan elapsedS) {
            _gameState.Update(tracer, elapsedS);

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

            var renderer = new GameObjectRenderer(geometries);
            foreach (var gameObject in _gameState.GameObjects) {
                renderer.Render(_gameState, gameObject);
            }
        }
    }
}
