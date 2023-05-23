using OpenTK;
using OpenTK.Graphics.OpenGL;
using Bearded.Graphics;

namespace Bearded.Photones.Rendering {
    /// <summary>
    /// Light vertex data used for rendering photons
    /// </summary>
    public struct PhotonVertexData : IVertexData {
        // add attributes and constructors here
        /// <summary>
        /// The position
        /// </summary>
        public Vector3 Position; // 12 bytes
        /// <summary>
        /// The radius
        /// </summary>
        public float Radius; // 4 bytes
        /// <summary>
        /// The color
        /// </summary>
        public Color Color; // 4 bytes

        /// <summary>
        /// This method returns the size of the vertex data struct in bytes
        /// </summary>
        public int Size() {
            return 20;
        }

        private static VertexAttribute[] vertexAttributes;

        /// <param name="position">The position.</param>
        /// <param name="radius">The radius.</param>
        /// <param name="color">The color.</param>
        public PhotonVertexData(Vector3 position, float radius, Color color) {
            Position = position;
            Radius = radius;
            Color = color;
        }

        private void setVertexAttributes() {
            vertexAttributes = new[]{
                new VertexAttribute("v_position", 3, VertexAttribPointerType.Float, Size(), 0),
                new VertexAttribute("v_radius", 1, VertexAttribPointerType.Float, Size(), 12),
                new VertexAttribute("v_color", 4, VertexAttribPointerType.UnsignedByte, Size(), 16,
                    true)
            };
        }

        /// <summary>
        /// Returns the vertex' <see cref="VertexAttributes" />
        /// </summary>
        /// <returns>
        /// Array of <see cref="VertexAttribute" />
        /// </returns>
        public VertexAttribute[] VertexAttributes() {
            if (vertexAttributes == null)
                setVertexAttributes();
            return vertexAttributes;
        }
    }
}
