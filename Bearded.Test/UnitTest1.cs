using System;
using System.Threading;
using System.Threading.Tasks;
using Bearded.Photones;
using Bearded.Utilities.IO;
using Xunit;

namespace Bearded.Test {
    public class UnitTest1 {
        [Fact]
        public void TestMethod1() {
            var logger = new Logger();

            logger.Info.Log("");
            logger.Info.Log("Creating game");
            var game = new PhotonesProgram(logger);

            logger.Info.Log("Running game");
            Task.Run(() => {
                Thread.Sleep(5000);
                game.Close();
            });
            game.Run(60);

            logger.Info.Log("Safely exited game");
        }
    }
}
