using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Bearded.Photones;
using Bearded.Utilities.IO;
using Xunit;

namespace Bearded.Test {
    public class PerformanceTests {

        private void DumpFrametimesCsv(string filename, string runname, List<int> frametimes) {
            // Open file and add line
            using (StreamWriter w = File.AppendText(filename)) {
                w.WriteLine(runname + ", " + string.Join(", ", frametimes));
            }
        }

        private void RunInstance(int fps, int frames, string title, bool collect) {
            List<int> frametimes = new List<int>(frames);
            var game = new PhotonesProgram(new Logger(),
                (g, e) => {
                    frametimes.Add((int)e.PerformanceStats.Frametime);
                    if (collect)
                        GC.Collect();
                    if (e.UpdateEventArgs.Frame > frames)
                        g.Close();
                });

            game.Run(fps);

            DumpFrametimesCsv("frametimes.csv", title, frametimes);
        }

        [Theory]
        [InlineData(10, false)]
        [InlineData(60, false)]
        [InlineData(0, false)]
        public void MeasureFrametimes(int fps, bool collect) {
            string title = $"{fps} FPS {(collect ? "with GC" : "")} ";
            RunInstance(fps, 400, title, collect);

            // Things to try:
            //Thread.CurrentThread.Priority = ThreadPriority.Highest;
            //GCSettings.LatencyMode = GCLatencyMode.LowLatency;
        }
    }
}
