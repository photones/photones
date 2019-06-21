using System;

namespace Bearded.Photones.Performance {
    /// <summary>
    /// Track stats on a some metric.
    /// </summary>
    public class VariableMonitor {
        public VariableStats Stats { get; private set; }
        public double CurrentValue { get; private set; }

        private VariableStats _currentStats;

        public VariableMonitor() {
            Refresh();
        }

        public void AddMeasurement(double value) {
            _currentStats = _currentStats.AddMeasurement(value, Stats);
        }

        /// <summary>
        /// Update Stats to reflect the statistics gathered between now and the previous Refresh call.
        /// </summary>
        public void Refresh() {
            Stats = _currentStats;
            _currentStats = VariableStats.CreateNew();
            CurrentValue = 0;
        }
    }
}
