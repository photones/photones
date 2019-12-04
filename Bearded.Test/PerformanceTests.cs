using System;
using System.Collections.Generic;
using System.IO;
using Bearded.Photones;
using Xunit;
using GameLogic;

namespace Bearded.Test {
    public class PerformanceTests {

        private void DumpFrametimesCsv(string filename, string runname, List<double> frametimes) {
            // Open file and add line
            using (StreamWriter w = File.AppendText(filename)) {
                w.WriteLine(runname + ", " + string.Join(", ", frametimes));
            }
        }

        private void RunInstance(int fps, int frames, string title, bool collect) {
            var frametimes = new List<double>(frames);
            var gameParameters = GameParameters.defaultParameters;
            var initialGameState = GameStateFactory.defaultScenario(gameParameters, 8);
            var game = new PhotonesProgram(initialGameState,
                (g, e) => {
                    frametimes.Add(e.PerformanceStats.FrameTime);
                    if (collect) {
                        GC.Collect();
                    }
                    if (e.UpdateEventArgs.Frame > frames) {
                        g.Close();
                    }
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
        }
    }
}
