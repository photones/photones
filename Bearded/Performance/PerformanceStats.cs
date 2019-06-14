namespace Bearded.Photones.Performance {
    public readonly struct PerformanceStats {
        public readonly double Fps;
        public readonly double FpsAvg;
        public readonly double FpsDev;
        public readonly double FpsMax;
        public readonly double FpsMin;

        public readonly double Frametime;
        public readonly double FrametimeAvg;
        public readonly double FrametimeDev;
        public readonly double FrametimeMax;
        public readonly double FrametimeMin;

        public readonly double ElapsedMax;
        public readonly double ElapsedMin;

        public PerformanceStats(double fps, double fpsAvg, double fpsDev, double fpsMax, double fpsMin, double frametime, double frametimeAvg, double frametimeDev, double frametimeMax, double frametimeMin, double elapsedMax, double elapsedMin) {
            Fps = fps;
            FpsAvg = fpsAvg;
            FpsDev = fpsDev;
            FpsMax = fpsMax;
            FpsMin = fpsMin;
            Frametime = frametime;
            FrametimeAvg = frametimeAvg;
            FrametimeDev = frametimeDev;
            FrametimeMax = frametimeMax;
            FrametimeMin = frametimeMin;
            ElapsedMax = elapsedMax;
            ElapsedMin = elapsedMin;
        }

        public string FpsString => $"{(int)FpsAvg}+/-{(int)FpsDev} [{(int)FpsMin},{(int)FpsMax}]";
        public string FrameTimeString => $"{(int)FrametimeAvg}+/-{(int)FrametimeDev} [{(int)FrametimeMin},{(int)FrametimeMax}]";
        public string ElapsedString => $"[{(int)(ElapsedMin)},{(int)(ElapsedMax)}]";
    }
}
