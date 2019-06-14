using amulware.Graphics;

namespace Bearded.Photones.Performance {
    public class BeardedUpdateEventArgs {

        public readonly PerformanceStats PerformanceStats;
        public readonly UpdateEventArgs UpdateEventArgs;

        public BeardedUpdateEventArgs(UpdateEventArgs e, PerformanceStats stats) {
            PerformanceStats = stats;
            UpdateEventArgs = e;
        }
    }
}
