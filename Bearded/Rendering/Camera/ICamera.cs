using OpenTK;

namespace Bearded.Photones.Rendering.Camera
{
    public interface ICamera
    {
        Matrix4 View { get; }
        Matrix4 Projection { get; }
    }
}
