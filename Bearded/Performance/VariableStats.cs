using System;

namespace Bearded.Photones.Performance {
    public readonly struct VariableStats {
        public readonly double Avg;
        public readonly double Dev;
        public readonly double Min;
        public readonly double Max;

        private readonly int _nrMeasurements;
        private readonly double _total;
        private readonly double _devTotal;

        private VariableStats(int nrMeasurements, double total, double devTotal, double min,
                double max) {
            _nrMeasurements = nrMeasurements;
            _total = total;
            _devTotal = devTotal;
            Avg = _total / _nrMeasurements;
            Dev = _devTotal / _nrMeasurements;
            Min = min;
            Max = max;
        }

        public static VariableStats CreateNew() {
            return new VariableStats(0, 0, 0, double.MaxValue, double.MinValue);
        }

        /// <param name="reference">The stats against which the deviation will be computed.</param>
        public VariableStats AddMeasurement(double value, VariableStats reference) {
            return new VariableStats(
                    _nrMeasurements + 1,
                    _total + value,
                    _devTotal + Math.Abs(value - reference.Avg),
                    Math.Min(Min, value), Math.Max(Max, value)
                );
        }
    }
}
