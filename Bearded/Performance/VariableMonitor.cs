using System;

namespace Bearded.Photones.Performance {
    /// <summary>
    /// Track stats on some metric.
    /// </summary>
    public class VariableMonitor {
        public VariableStats Stats { get; private set; }
        public double CurrentValue { get; private set; }

        private VariableStats _measurements;

        public VariableMonitor() {
            Refresh();
        }

        public void AddMeasurement(double value) {
            _measurements = _measurements.AddMeasurement(value, Stats);
        }

        /// <summary>
        /// Update Stats to reflect the statistics gathered between now and the previous Refresh call.
        /// </summary>
        public void Refresh() {
            Stats = _measurements;
            _measurements = VariableStats.CreateNew();
            CurrentValue = 0;
        }
    }
}
