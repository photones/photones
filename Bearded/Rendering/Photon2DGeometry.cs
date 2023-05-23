using OpenTK;
using Bearded.Graphics;

namespace Bearded.Photones.Rendering {
    /// <summary>
    /// Geometry that draws photons in two dimensional space
    /// </summary>
    public class Photon2DGeometry {
        /// <summary>
        /// The color to draw with
        /// </summary>
        public Color Color = Color.White;

        public ExpandingVertexSurface<PhotonVertexData> Surface { get; private set; }

        public Photon2DGeometry(ExpandingVertexSurface<PhotonVertexData> surface) {
            Surface = surface;
        }

        public void DrawParticle(Vector3 position, float radius, Color color) {
            Surface.AddVertex(new PhotonVertexData(position, radius, color));
        }

        public void DrawParticle(Vector2 position, float radius, Color color) {
            DrawParticle(new Vector3(position.X, position.Y, 0), radius, color);
        }
    }
}
