using amulware.Graphics;
using Bearded.Photones.Performance;
using GameLogic;

namespace Bearded.Photones {
    public class BeardedUpdateEventArgs {

        public readonly UpdateEventArgs UpdateEventArgs;
        public readonly InputActions.T InputActions;
        public readonly PerformanceSummary PerformanceStats;

        public BeardedUpdateEventArgs(
            UpdateEventArgs e, InputActions.T inputActions, PerformanceSummary stats
        ) {
            UpdateEventArgs = e;
            InputActions = inputActions;
            PerformanceStats = stats;
        }
    }
}
