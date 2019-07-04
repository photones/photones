using Bearded.Photones.Rendering;
using GameLogic;
using amulware.Graphics;

namespace Bearded.Photones.GameUI {
    class GameObjectRenderer {
        private readonly GeometryManager geometries;

        public GameObjectRenderer(GeometryManager geometries) {
            this.geometries = geometries;
        }

        public void Render(GameObject<GameState> value) {
            value.Visit(RenderPhoton, RenderPlanet);
        }

        public void RenderPhoton(PhotonData value) {
            var photoncolor = Color.DarkGoldenrod;
            geometries.PhotonGeometry.DrawParticle(value.Position.NumericValue, photoncolor);
        }

        public void RenderPlanet(PlanetData value) {
        }
    }
}
