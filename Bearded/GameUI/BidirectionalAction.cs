using Bearded.Utilities;
using Bearded.Utilities.Input;

namespace Bearded.Photones {
    class BidirectionalAction : IAction {
        public readonly IAction Up;
        public readonly IAction Down;

        public BidirectionalAction(IAction up, IAction down) {
            Up = up;
            Down = down;
        }

        public bool Hit => Up.Hit || Down.Hit;
        public bool Active => Up.Active || Down.Active;
        public bool Released => (Up.Released || Down.Released) && !Active;
        public bool IsAnalog => Up.IsAnalog || Down.IsAnalog;
        public float AnalogAmount => Up.AnalogAmount - Down.AnalogAmount;

        public bool Equals(IAction other) => this.IsSameAs(other);
    }
}
