using amulware.Graphics;
using Bearded.Photones.Screens;
using OpenTK.Graphics.OpenGL;

namespace Bearded.Photones.Rendering {
    class FrameCompositor {
        private readonly SurfaceManager surfaces;

        public FrameCompositor(SurfaceManager surfaces) {
            this.surfaces = surfaces;
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

            surfaces.ViewMatrix.Matrix = layer.ViewMatrix;
            surfaces.ProjectionMatrix.Matrix = layer.ProjectionMatrix;

            surfaces.FreshmanFontSurface.Render();
            surfaces.ConsolasFontSurface.Render();
            surfaces.SpriteSurface.Render();
            surfaces.PhotonSurface.Render();
        }

        public void FinalizeFrame() { }
    }
}
