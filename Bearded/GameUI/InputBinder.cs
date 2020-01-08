using Bearded.Utilities.Input;
using GameLogic;

namespace Bearded.Photones {

    static class InputBinder {
        public static InputActions.T DefaultInputActions(InputManager inputManager) {
            var moveVertical = new BidirectionalAction(
                inputManager.Actions.Keyboard.FromKey(OpenTK.Input.Key.Up),
                inputManager.Actions.Keyboard.FromKey(OpenTK.Input.Key.Down)
            );
            var moveHorizontal = new BidirectionalAction(
                inputManager.Actions.Keyboard.FromKey(OpenTK.Input.Key.Right),
                inputManager.Actions.Keyboard.FromKey(OpenTK.Input.Key.Left)
            );
            var modA = new BidirectionalAction(
                inputManager.Actions.Keyboard.FromKey(OpenTK.Input.Key.W),
                inputManager.Actions.Keyboard.FromKey(OpenTK.Input.Key.Q)
            );
            var modB = new BidirectionalAction(
                inputManager.Actions.Keyboard.FromKey(OpenTK.Input.Key.S),
                inputManager.Actions.Keyboard.FromKey(OpenTK.Input.Key.A)
            );
            var modC = new BidirectionalAction(
                inputManager.Actions.Keyboard.FromKey(OpenTK.Input.Key.X),
                inputManager.Actions.Keyboard.FromKey(OpenTK.Input.Key.Z)
            );
            var intModD = new BidirectionalAction(
                inputManager.Actions.Keyboard.FromKey(OpenTK.Input.Key.R),
                inputManager.Actions.Keyboard.FromKey(OpenTK.Input.Key.E)
            );
            var playerActions = new InputActions.PlayerActions(moveVertical, moveHorizontal);
            var modTime = new BidirectionalAction(
                inputManager.Actions.Keyboard.FromKey(OpenTK.Input.Key.PageUp),
                inputManager.Actions.Keyboard.FromKey(OpenTK.Input.Key.PageDown)
            );
            return new InputActions.T(modTime, modA, modB, modC, intModD, playerActions);
        }
    }
}
