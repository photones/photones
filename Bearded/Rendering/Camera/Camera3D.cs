using System;
using OpenTK;
using Bearded.Utilities.Math;

namespace Bearded.Photones.Rendering.Camera
{
    public class Camera3D : ICamera
    {
        public Matrix4 View { get; private set; }
        public Matrix4 Projection { get; private set; }
        public Vector3 Eye { get; set; }
        public Vector3 Focus { get; set; }
        public Vector3 Up { get; private set; }

        public Camera3D()
            : this(Vector3.UnitZ, Vector3.Zero, Vector3.UnitY) { } // xy as 2D, z from the screen

        public Camera3D(Vector3 camEye, Vector3 camFocus, Vector3 camUp) {
            Eye = camEye;
            Focus = camFocus;
            Up = camUp;
            updateProjection();
            updateView();
        }

        void updateView() {
            updateView(Vector3.Zero);
        }
        void updateView(Vector3 offset) {
            View = Matrix4.LookAt(Eye + offset, Focus + offset, Up);
        }

        void updateProjection() {
            Projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver2, PhotonesProgram.WIDTH / PhotonesProgram.HEIGHT, 1f, 100f);
        }

        public void Move(Vector3 direction) {
            Eye += direction;
            Focus += direction;
            updateView();
        }
        public void MoveTo(Vector3 position) {
            Move(position - Eye);
        }
        public void MoveFocusTo(Vector3 position) {
            MoveTo(position - Focus + Eye);
        }

        public void RotateDeg(float degrees) {
            RotateRad(MathHelper.DegreesToRadians(degrees));
        }
        public void RotateRad(float radians) {
            Vector3 eye2focus = Focus - Eye;
            Focus = Eye + Vector3.TransformVector(eye2focus, Matrix4.CreateRotationY(radians));

            updateView();
        }

        public void RotateAroundFocusDeg(float degrees) {
            RotateAroundFocusRad(MathHelper.DegreesToRadians(degrees));
        }
        public void RotateAroundFocusRad(float radians) {
            Vector3 focus2eye = Eye - Focus;
            Eye = Focus + Vector3.TransformVector(focus2eye, Matrix4.CreateRotationY(radians));

            updateView();
        }

        public void ChangeDistance(float distanceChange) {
            if (distanceChange != 0) {
                Vector3 focus2eye = Eye - Focus;
                float distance = focus2eye.LengthFast;
                SetDistance(distance + distanceChange);
            }
        }

        public void SetDistance(float distance) {
            distance = distance.Clamped(1.01f, 500f);
            Vector3 focus2eye = Eye - Focus;
            focus2eye.NormalizeFast();
            Eye = Focus + focus2eye * distance;

            updateView();
        }
    }
}
