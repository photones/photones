﻿using OpenTK;
using OpenTK.Graphics.OpenGL;
using amulware.Graphics;

namespace Bearded.Photones.Rendering
{
    /// <summary>
    /// Light vertex data used for rendering textured vertices, like sprites.
    /// </summary>
    public struct FastParticleVertexData : IVertexData
    {
        // add attributes and constructors here
        /// <summary>
        /// The position
        /// </summary>
        public Vector3 Position; // 12 bytes
        /// <summary>
        /// The color
        /// </summary>
        public Color Color; // 4 bytes

        /// <summary>
        /// This method returns the size of the vertex data struct in bytes
        /// </summary>
        /// <returns>
        /// Struct's size in bytes
        /// </returns>
        public int Size()
        {
            return 16;
        }

        static private VertexAttribute[] vertexAttributes;

        /// <param name="position">The position.</param>
        /// <param name="color">The color.</param>
        public FastParticleVertexData(Vector3 position, Color color)
        {
            this.Position = position;
            this.Color = color;
        }

        static private void setVertexAttributes()
        {
            vertexAttributes = new VertexAttribute[]{
                new VertexAttribute("v_position", 3, VertexAttribPointerType.Float, 24, 0),
                new VertexAttribute("v_color", 4, VertexAttribPointerType.UnsignedByte, 24, 20, true)
            };
        }

        /// <summary>
        /// Returns the vertex' <see cref="VertexAttributes" />
        /// </summary>
        /// <returns>
        /// Array of <see cref="VertexAttribute" />
        /// </returns>
        public VertexAttribute[] VertexAttributes()
        {
            if (vertexAttributes == null)
                setVertexAttributes();
            return vertexAttributes;
        }
    }
}