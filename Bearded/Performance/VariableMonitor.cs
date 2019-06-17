using System;

namespace Bearded.Photones.Performance {
    public class VariableMonitor {
        public VariableStats Stats { get; private set; }
        public double Current { get; private set; }

        private VariableStats _currentStats; // The statistics being constructed
        private long _nrOfMeasurements;

        public VariableMonitor() {
            Init();
        }

        private void Init() {
            _currentStats = new VariableStats(0, 0, double.MaxValue, double.MinValue);
            _nrOfMeasurements = 0;
            Current = 0;
        }

        public void AddMeasurement(double value) {
            Current = value;
            var avg = _currentStats.Avg + value;
            var dev = _currentStats.Dev + Math.Abs(value - Stats.Avg);
            var min = Math.Min(_currentStats.Min, value);
            var max = Math.Max(_currentStats.Max, value);
            _currentStats = new VariableStats(avg, dev, min, max);
            _nrOfMeasurements++;
        }

        public void UpdateStats() {
            Stats = new VariableStats(
                _currentStats.Avg / _nrOfMeasurements,
                _currentStats.Dev / _nrOfMeasurements,
                _currentStats.Min,
                _currentStats.Max
                );
            Init();
        }
    }
}
