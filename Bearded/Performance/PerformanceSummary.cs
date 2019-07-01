namespace Bearded.Photones.Performance {
    public readonly struct PerformanceSummary {
        public readonly VariableStats FrameTimeStats;
        public readonly VariableStats ElapsedTimeStats;
        public readonly double FrameTime;
        public readonly int NrGameObjects;

        public PerformanceSummary(VariableStats frameTimeStats, VariableStats elapsedTimeStats, double frametime, int nrGameObjects) {
            FrameTimeStats = frameTimeStats;
            ElapsedTimeStats = elapsedTimeStats;
            FrameTime = frametime;
            NrGameObjects = nrGameObjects;
        }

        public string FrameTimeString => $"Frame time: {FrameTimeStats.Avg:0.0}+/-{FrameTimeStats.Dev:0.0} [{FrameTimeStats.Min:0.0},{FrameTimeStats.Max:0.0}]";
        public string ElapsedTimeString => $"Elapsed time: {ElapsedTimeStats.Avg:0.0}+/-{ElapsedTimeStats.Dev:0.0} [{ElapsedTimeStats.Min:0.0},{ElapsedTimeStats.Max:0.0}]";
        public string NrGameObjectsString => $"Game objects: {NrGameObjects}";
    }
}
