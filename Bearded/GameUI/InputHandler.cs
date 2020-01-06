using Bearded.Photones.Rendering.Camera;
using Bearded.Photones.UI;
using GameLogic;

namespace Bearded.Photones.GameUI {
    static class InputHandler {

        public static bool HandleInput(GameState gameState, Camera3D camera, InputState inputState) {
            if (inputState.InputManager.IsKeyPressed(OpenTK.Input.Key.PageUp)) {
                IncrementGameSpeed(gameState);
            }
            if (inputState.InputManager.IsKeyPressed(OpenTK.Input.Key.PageDown)) {
                DecrementGameSpeed(gameState);
            }
            if (inputState.InputManager.IsKeyPressed(OpenTK.Input.Key.W)) {
                IncrementModA(gameState);
            }
            if (inputState.InputManager.IsKeyPressed(OpenTK.Input.Key.Q)) {
                DecrementModA(gameState);
            }
            if (inputState.InputManager.IsKeyPressed(OpenTK.Input.Key.S)) {
                IncrementModB(gameState);
            }
            if (inputState.InputManager.IsKeyPressed(OpenTK.Input.Key.A)) {
                DecrementModB(gameState);
            }
            if (inputState.InputManager.IsKeyPressed(OpenTK.Input.Key.X)) {
                IncrementModC(gameState);
            }
            if (inputState.InputManager.IsKeyPressed(OpenTK.Input.Key.Z)) {
                DecrementModC(gameState);
            }
                
            camera.ChangeDistance(-inputState.InputManager.DeltaScroll * .1f);
            return true;
        }

        private static double Decrease(double oldValue, double step) {
            return System.Math.Max(0.0, oldValue - step);
        }

        private static double Increase(double oldValue, double step) {
            return System.Math.Max(0.0, oldValue + step);
        }

        public static void DecrementGameSpeed(GameState gameState) {
            var oldValue = gameState.GameParameters.TimeModifier;
            var newParams = gameState.GameParameters.WithTimeModifier(Decrease(oldValue, 0.01));
            gameState.SetGameParameters(newParams);
        }

        public static void IncrementGameSpeed(GameState gameState) {
            var oldValue = gameState.GameParameters.TimeModifier;
            var newParams = gameState.GameParameters.WithTimeModifier(Increase(oldValue, 0.01));
            gameState.SetGameParameters(newParams);
        }

        public static void DecrementModA(GameState gameState) {
            var oldValue = gameState.GameParameters.ModA;
            var newParams = gameState.GameParameters.WithModA(Decrease(oldValue, 0.01));
            gameState.SetGameParameters(newParams);
        }

        public static void IncrementModB(GameState gameState) {
            var oldValue = gameState.GameParameters.ModB;
            var newParams = gameState.GameParameters.WithModB(Increase(oldValue, 0.01));
            gameState.SetGameParameters(newParams);
        }

        public static void DecrementModB(GameState gameState) {
            var oldValue = gameState.GameParameters.ModB;
            var newParams = gameState.GameParameters.WithModB(Decrease(oldValue, 0.01));
            gameState.SetGameParameters(newParams);
        }

        public static void IncrementModA(GameState gameState) {
            var oldValue = gameState.GameParameters.ModA;
            var newParams = gameState.GameParameters.WithModA(Increase(oldValue, 0.01));
            gameState.SetGameParameters(newParams);
        }

        public static void DecrementModC(GameState gameState) {
            var oldValue = gameState.GameParameters.ModC;
            var newParams = gameState.GameParameters.WithModC(Decrease(oldValue, 0.01));
            gameState.SetGameParameters(newParams);
        }

        public static void IncrementModC(GameState gameState) {
            var oldValue = gameState.GameParameters.ModC;
            var newParams = gameState.GameParameters.WithModC(Increase(oldValue, 0.01));
            gameState.SetGameParameters(newParams);
        }
    }
}
