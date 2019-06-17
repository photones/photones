namespace Bearded.Photones.Performance {
    public readonly struct VariableStats {
        public readonly double Avg;
        public readonly double Dev;
        public readonly double Min;
        public readonly double Max;

        public VariableStats(double avg, double dev, double min, double max) {
            Avg = avg;
            Dev = dev;
            Min = min;
            Max = max;
        }
    }
}
