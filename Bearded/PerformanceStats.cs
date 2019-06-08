namespace Bearded.Photones {
    public struct PerformanceStats {
        public readonly double FpsAvg;
        public readonly double FpsDev;
        public readonly double FpsMax;
        public readonly double FpsMin;

        public readonly double FrametimeAvg;
        public readonly double FrametimeDev;
        public readonly double FrametimeMax;
        public readonly double FrametimeMin;

        public PerformanceStats(double fpsAvg, double fpsDev, double fpsMax, double fpsMin, double frametimeAvg, double frametimeDev, double frametimeMax, double frametimeMin) {
            FpsAvg = fpsAvg;
            FpsDev = fpsDev;
            FpsMax = fpsMax;
            FpsMin = fpsMin;
            FrametimeAvg = frametimeAvg;
            FrametimeDev = frametimeDev;
            FrametimeMax = frametimeMax;
            FrametimeMin = frametimeMin;
        }

        public string FpsString => $"{(int)FpsAvg}+/-{(int)FpsDev} [{(int)FpsMin},{(int)FpsMax}]";
        public string FrameTimeString => $"{(int)FrametimeAvg}+/-{(int)FrametimeDev} [{(int)FrametimeMin},{(int)FrametimeMax}]";
    }
}
