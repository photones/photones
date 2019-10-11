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

        public static Color PlayerColor(byte playerIndex) {
            switch (playerIndex) {
                case 0: return Color.DarkGoldenrod;
                case 1: return Color.HotPink;
            }
            throw new NotImplementedException();
        }

        public void RenderPhoton(PhotonData value) {
            var color = PlayerColor(value.PlayerId);
            geometries.PhotonGeometry.DrawParticle(value.Position.NumericValue, value.Size.NumericValue, color);
        }

        public void RenderPlanet(PlanetData value) {
            var color = PlayerColor(value.PlayerId);
            // Draw Planets as big photons
            geometries.PhotonGeometry.DrawParticle(value.Position.NumericValue, value.Size.NumericValue, color);
        }
    }
}
