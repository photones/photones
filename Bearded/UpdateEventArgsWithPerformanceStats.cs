using amulware.Graphics;

namespace Bearded.Photones {
    class UpdateEventArgsWithPerformanceStats : UpdateEventArgs {

        public readonly PerformanceStats Stats;

        public UpdateEventArgsWithPerformanceStats(UpdateEventArgs e, PerformanceStats stats) : base(e) {
            Stats = stats;
        }
    }
}
