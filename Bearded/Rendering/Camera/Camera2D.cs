using OpenTK;

namespace Bearded.Photones.Rendering.Camera {
    public class Camera2D : ICamera {
        public Matrix4 View { get; private set; }
        public Matrix4 Projection { get; private set; }

        public Camera2D() {
            float w = PhotonesProgram.WIDTH;
            float h = PhotonesProgram.HEIGHT;

            View = Matrix4.CreateTranslation(-w / 2f, -h / 2f, 0)
                * new Matrix4(Vector4.UnitX, -Vector4.UnitY, Vector4.UnitZ, Vector4.UnitW)
                * Matrix4.LookAt(-2f * Vector3.UnitZ, Vector3.UnitZ, -Vector3.UnitY);

            Projection = Matrix4.CreatePerspectiveOffCenter(-w / 4f, w / 4f, h / 4f, -h / 4f, 1f, 64f);
        }
    }
}
