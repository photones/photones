using OpenTK;
using amulware.Graphics;

namespace Bearded.Photones.Rendering {
    /// <summary>
    /// Geometry that draws photons in two dimensional space
    /// </summary>
    public class Photon2DGeometry {
        /// <summary>
        /// The color to draw with
        /// </summary>
        public Color Color = Color.Pink;

        public ExpandingVertexSurface<PhotonVertexData> Surface { get; private set; }

        public Photon2DGeometry(ExpandingVertexSurface<PhotonVertexData> surface) {
            Surface = surface;
        }

        public void DrawParticle(Vector3 position, Color color) {
            Surface.AddVertex(new PhotonVertexData(position, color));
        }

        public void DrawParticle(Vector2 position, Color? color = default) {
            var c = color ?? Color;
            this.DrawParticle(new Vector3(position.X, position.Y, 0), c);
        }
    }
}
