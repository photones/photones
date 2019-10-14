using Bearded.Photones.Rendering;
using GameLogic;

namespace Bearded.Photones.GameUI {
    class GameObjectRenderer {
        private readonly GeometryManager _geometries;
        private GameState _gameState;

        public GameObjectRenderer(GeometryManager geometries) {
            _geometries = geometries;
        }

        public void Render(GameState gameState, GameObject<GameState> value) {
            _gameState = gameState;
            value.Visit(RenderPhoton, RenderPlanet);
        }

        public void RenderPhoton(PhotonData state) {
            var player = Player.getPlayerById(_gameState, state.PlayerId);
            var color = player.State.Color;
            var position = state.Position.NumericValue;
            var size = state.Size.NumericValue;
            _geometries.PhotonGeometry.DrawParticle(position, size, color);
        }

        public void RenderPlanet(PlanetData state) {
            var player = Player.getPlayerById(_gameState, state.PlayerId);
            var color = player.State.Color;
            var position = state.Position.NumericValue;
            var size = state.Size.NumericValue;
            // Draw Planets as big photons
            _geometries.PhotonGeometry.DrawParticle(position, size, color);
        }
    }
}
