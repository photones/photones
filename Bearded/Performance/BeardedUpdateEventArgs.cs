using Bearded.Graphics;

namespace Bearded.Photones.Performance {
    public class BeardedUpdateEventArgs {

        public readonly PerformanceSummary PerformanceStats;
        public readonly UpdateEventArgs UpdateEventArgs;

        public BeardedUpdateEventArgs(UpdateEventArgs e, PerformanceSummary stats) {
            PerformanceStats = stats;
            UpdateEventArgs = e;
        }
    }
}
