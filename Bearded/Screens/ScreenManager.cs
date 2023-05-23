﻿using System.Collections.Concurrent;
using System.Collections.Generic;
using Bearded.Graphics;
using Bearded.Photones.Performance;
using Bearded.Photones.UI;
using Bearded.Utilities.Input;
using GameLogic;

namespace Bearded.Photones.Screens {
    class ScreenManager : ScreenLayerCollection {
        private readonly InputManager _inputManager;
        private readonly List<char> _pressedCharacterList = new List<char>();
        private readonly ConcurrentQueue<char> _pressedCharacterQueue = new ConcurrentQueue<char>();
        private readonly IReadOnlyList<char> _pressedCharacterInterface;

        public ScreenManager(InputManager inputManager) {
            _inputManager = inputManager;
            _pressedCharacterInterface = _pressedCharacterList.AsReadOnly();
        }

        public void Update(BeardedUpdateEventArgs args) {
            handleInput(args.UpdateEventArgs);
            UpdateAll(args);
        }

        private void handleInput(UpdateEventArgs args) {
            while (_pressedCharacterQueue.TryDequeue(out char c)) {
                _pressedCharacterList.Add(c);
            }

            var inputState = new InputState(_pressedCharacterInterface, _inputManager);

            PropagateInput(args, inputState);

            _pressedCharacterList.Clear();
        }

        public void RegisterPressedCharacter(char c) {
            _pressedCharacterQueue.Enqueue(c);
        }
    }
}
