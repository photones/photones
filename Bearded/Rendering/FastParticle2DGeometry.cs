using OpenTK;
using amulware.Graphics;

namespace Bearded.Photones.Rendering
{
    /// <summary>
    /// Geometry that draws particles using sprites in two dimensional space
    /// </summary>
    public class FastParticle2DGeometry
    {
        /// <summary>
        /// The color to draw with
        /// </summary>
        public Color Color = Color.Pink;

        public ExpandingVertexSurface<FastParticleVertexData> Surface { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sprite2DGeometry"/> class.
        /// </summary>
        /// <param name="surface">The surface used for drawing</param>
        public FastParticle2DGeometry(ExpandingVertexSurface<FastParticleVertexData> surface)
        {
            Surface = surface;
        }

        /// <summary>
        /// Draws a sprite.
        /// </summary>
        public void DrawSprite(Vector3 position)
        {
            Surface.AddVertex(new FastParticleVertexData(position, Color));
        }

        /// <summary>
        /// Draws a sprite.
        /// </summary>
        /// <param name="position">The coordinates to draw the sprite at. The sprite is drawn centered around this point.</param>
        public void DrawSprite(Vector2 position)
        {
            this.DrawSprite(new Vector3(position.X, position.Y, 0));
        }
    }
}
