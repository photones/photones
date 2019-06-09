using amulware.Graphics;

namespace Bearded.Photones.Performance {
    class BeardedUpdateEventArgs : UpdateEventArgs {

        public readonly PerformanceStats PerformanceStats;

        public BeardedUpdateEventArgs(UpdateEventArgs e, PerformanceStats stats) : base(e) {
            PerformanceStats = stats;
        }
    }
}
