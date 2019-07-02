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

        public void Update(Tracer tracer, UpdateEventArgs uea) {
            _gameState.Update(tracer, uea);
        }

        public void Draw(GeometryManager geometries) {
            // Coord system
            var coordcolor = Color.Red;
            geometries.PhotonGeometry.DrawParticle(new Vector2(0, 0), coordcolor);
            geometries.PhotonGeometry.DrawParticle(new Vector2(1, 1), coordcolor);
            geometries.PhotonGeometry.DrawParticle(new Vector2(1, -1), coordcolor);
            geometries.PhotonGeometry.DrawParticle(new Vector2(-1, 1), coordcolor);
            geometries.PhotonGeometry.DrawParticle(new Vector2(-1, -1), coordcolor);

            var renderer = new GameObjectRenderer(geometries);
            foreach (var gameObject in _gameState.GameObjects) {
                renderer.Render(gameObject);
            }
        }
    }
}
