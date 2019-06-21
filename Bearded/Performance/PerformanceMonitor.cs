using System;
using System.Diagnostics;

namespace Bearded.Photones.Performance {

    /// <summary>
    /// Everything is computed in milliseconds.
    /// </summary>
    partial class PerformanceMonitor {

        private const double UPDATE_INTERVAL = 1000;

        private Stopwatch _watch;
        private double _elapsedSinceUpdate;

        public VariableMonitor FrameTime = new VariableMonitor();
        public VariableMonitor ElapsedTime = new VariableMonitor();

        public void StartFrame(double elapsedTimeInS) {
            double elapsed = elapsedTimeInS * 1000;
            _elapsedSinceUpdate += elapsed;

            ElapsedTime.AddMeasurement(elapsed);

            _watch = Stopwatch.StartNew();
        }

        public void EndFrame() {
            if (_watch == null) {
                return;
            }

            _watch.Stop();

            FrameTime.AddMeasurement(_watch.ElapsedMilliseconds);

            if (_elapsedSinceUpdate >= UPDATE_INTERVAL) {
                _elapsedSinceUpdate = 0;
                FrameTime.Refresh();
                ElapsedTime.Refresh();
            }
        }

        public PerformanceSummary GetPerformanceSummary() {
            return new PerformanceSummary(FrameTime.Stats, ElapsedTime.Stats, FrameTime.CurrentValue);
        }
    }
}
