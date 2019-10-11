using Bearded.Photones.Rendering;
using GameLogic;
using amulware.Graphics;
using System;

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
            var color = value.Player.Color;
            var position = value.Position.NumericValue;
            var size = value.Size.NumericValue;
            geometries.PhotonGeometry.DrawParticle(position, size, color);
        }

        public void RenderPlanet(PlanetData value) {
            var color = value.Player.Color;
            var position = value.Position.NumericValue;
            var size = value.Size.NumericValue;
            // Draw Planets as big photons
            geometries.PhotonGeometry.DrawParticle(position, size, color);
        }
    }
}
