using System.Collections.Generic;
using Bearded.Utilities.Input;

namespace Bearded.Photones.UI {
    class InputState {
        public IReadOnlyList<char> PressedCharacters { get; }
        public InputManager InputManager { get; }

        public InputState(IReadOnlyList<char> pressedCharacters, InputManager inputManager) {
            PressedCharacters = pressedCharacters;
            InputManager = inputManager;
        }
    }
}