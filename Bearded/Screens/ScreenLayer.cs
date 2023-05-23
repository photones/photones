﻿using Bearded.Graphics;
using Bearded.Photones.Performance;
using Bearded.Photones.Rendering;
using Bearded.Photones.UI;
using Bearded.Utilities;
using OpenTK;
using GameLogic;
using OpenTK.Mathematics;
using System;

namespace Bearded.Photones.Screens {
    abstract class ScreenLayer : IScreenLayer {
        private const float fovy = MathF.PI/2;
        private const float zNear = .1f;
        private const float zFar = 1024f;

        protected readonly ScreenLayerCollection Parent;

        protected ViewportSize ViewportSize { get; private set; }

        public abstract Matrix4 ViewMatrix { get; }

        public virtual Matrix4 ProjectionMatrix {
            get {
                var yMax = zNear * MathF.Tan(.5f * fovy);
                var yMin = -yMax;
                var xMax = yMax * ViewportSize.AspectRatio;
                var xMin = yMin * ViewportSize.AspectRatio;
                return Matrix4.CreatePerspectiveOffCenter(xMin, xMax, yMin, yMax, zNear, zFar);
            }
        }

        protected ScreenLayer(ScreenLayerCollection parent) {
            Parent = parent;
        }

        public void OnResize(ViewportSize newSize) {
            ViewportSize = newSize;
            OnViewportSizeChanged();
        }

        public virtual bool HandleInput(UpdateEventArgs args, InputState inputState) => true;
        public abstract void Update(BeardedUpdateEventArgs args);
        public abstract void Draw();
        protected virtual void OnViewportSizeChanged() { }

        public void Render(RenderContext context) {
            context.Compositor.RenderLayer(this);
        }

        protected void Destroy() {
            Parent.RemoveScreenLayer(this);
        }
    }
}
