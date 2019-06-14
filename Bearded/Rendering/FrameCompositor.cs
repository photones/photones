using amulware.Graphics;
using Bearded.Photones.Screens;
using OpenTK.Graphics.OpenGL;

namespace Bearded.Photones.Rendering {
    class FrameCompositor {
        private readonly SurfaceManager _surfaces;

        public FrameCompositor(SurfaceManager surfaces) {
            _surfaces = surfaces;
        }

        public void PrepareForFrame() {
            var argb = Color.Black;
            GL.ClearColor(argb.R / 255f, argb.G / 255f, argb.B / 255f, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Disable(EnableCap.DepthTest);
            GL.DepthMask(false);
            GL.CullFace(CullFaceMode.FrontAndBack);

            GL.Enable(EnableCap.Blend);
            SurfaceBlendSetting.PremultipliedAlpha.Set(null);
        }

        public void RenderLayer(ScreenLayer layer) {
            layer.Draw();

            _surfaces.ViewMatrix.Matrix = layer.ViewMatrix;
            _surfaces.ProjectionMatrix.Matrix = layer.ProjectionMatrix;

            _surfaces.FreshmanFontSurface.Render();
            _surfaces.ConsolasFontSurface.Render();
            _surfaces.SpriteSurface.Render();
            _surfaces.PhotonSurface.Render();
        }

        public void FinalizeFrame() { }
    }
}
