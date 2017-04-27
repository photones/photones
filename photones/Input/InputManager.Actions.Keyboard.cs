using System;
using System.Collections.Generic;
using System.Linq;
using Bearded.Photones.Input.Actions;
using OpenTK.Input;

namespace Bearded.Photones.Input
{
    partial class InputManager
    {
        public partial struct ActionConstructor
        {
            public KeyboardActions Keyboard => new KeyboardActions(manager);
        }

        public struct KeyboardActions
        {
            private readonly InputManager manager;

            public KeyboardActions(InputManager inputManager) {
                manager = inputManager;
            }

            public IAction FromKey(Key key) => new KeyboardAction(manager, key);

            public IAction FromString(string name) {
                var lower = name.ToLowerInvariant().Trim();
                if (!lower.StartsWith("keyboard:"))
                    return null;

                var keyName = name.Substring(9).Trim();

                var key = (Key)Enum.Parse(typeof(Key), keyName, true);

                if (key == Key.Unknown)
                    throw new ArgumentException("Keyboard key name unknown.", nameof(name));

                return new KeyboardAction(manager, key);
            }

            public IEnumerable<IAction> All {
                get {
                    var inputManager = manager;
                    return ((Key[])Enum.GetValues(typeof(Key))).Select(k => new KeyboardAction(inputManager, k));
                }
            }
        }
    }
}