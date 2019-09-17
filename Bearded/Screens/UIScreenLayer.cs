using System.Collections.Generic;
using amulware.Graphics;
using Bearded.Photones.Performance;
using Bearded.Photones.Rendering;
using Bearded.Photones.UI;
using Bearded.Photones.UI.Components;
using OpenTK;
using GameLogic;

namespace Bearded.Photones.Screens {
    abstract class UIScreenLayer : ScreenLayer {
        private Matrix4 _viewMatrix;
        private Matrix4 _projectionMatrix;
        public override Matrix4 ViewMatrix => _viewMatrix;
        public override Matrix4 ProjectionMatrix => _projectionMatrix;

        protected GeometryManager Geometries { get; }
        protected Screen Screen { get; }

        private readonly List<UIComponent> _components = new List<UIComponent>();

        protected UIScreenLayer(ScreenLayerCollection parent, GeometryManager geometries)
                : base(parent) {
            Geometries = geometries;
            Screen = Screen.GetCanvas();
        }

        public override void Update(Tracer tracer, BeardedUpdateEventArgs args) {
            _components.ForEach(c => c.Update(args.UpdateEventArgs));
        }

        public override bool HandleInput(UpdateEventArgs args, InputState inputState) {
            _components.ForEach(c => c.HandleInput(inputState));
            return true;
        }

        public override void Draw() {
            _components.ForEach(c => c.Draw(Geometries));
        }

        protected void AddComponent(UIComponent component) {
            _components.Add(component);
        }

        protected override void OnViewportSizeChanged() {
            // These 2D matrices create a pixel perfect projection with a scale
            // from 1:1 from the z=0 plane to the screen.
            var (w, h) = ViewportSize;
            _viewMatrix = Matrix4.CreateTranslation(-w / 2f, -h / 2f, 0)
                * new Matrix4(Vector4.UnitX, -Vector4.UnitY, Vector4.UnitZ, Vector4.UnitW)
                * Matrix4.LookAt(-2f * Vector3.UnitZ, Vector3.UnitZ, -Vector3.UnitY);
            _projectionMatrix = Matrix4.CreatePerspectiveOffCenter(-w / 4f,
                w / 4f,
                h / 4f,
                -h / 4f,
                1f,
                64f);

            Screen.OnResize(ViewportSize);
        }
    }
}
