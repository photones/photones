using OpenTK;
using amulware.Graphics;

namespace Bearded.Photones.Rendering
{
    /// <summary>
    /// Geometry that draws particles using sprites in two dimensional space
    /// </summary>
    public class Photon2DGeometry
    {
        /// <summary>
        /// The color to draw with
        /// </summary>
        public Color Color = Color.Pink;

        public ExpandingVertexSurface<PhotonVertexData> Surface { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sprite2DGeometry"/> class.
        /// </summary>
        /// <param name="surface">The surface used for drawing</param>
        public Photon2DGeometry(ExpandingVertexSurface<PhotonVertexData> surface)
        {
            Surface = surface;
        }

        /// <summary>
        /// Draws a sprite.
        /// </summary>
        public void DrawParticle(Vector3 position)
        {
            Surface.AddVertex(new PhotonVertexData(position, Color));
        }

        /// <summary>
        /// Draws a sprite.
        /// </summary>
        /// <param name="position">The coordinates to draw the sprite at. The sprite is drawn centered around this point.</param>
        public void DrawParticle(Vector2 position)
        {
            this.DrawParticle(new Vector3(position.X, position.Y, 0));
        }
    }
}
