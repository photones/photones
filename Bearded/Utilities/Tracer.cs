using Bearded.Utilities.IO;
using GameLogic;

namespace Bearded.Photones.Utilities {
    class Tracer : ITracer {
        private readonly Logger _logger;
        private readonly GameStatistics _gameStatistics;

        public Tracer(Logger logger, GameStatistics gameStatistics) {
            _logger = logger;
            _gameStatistics = gameStatistics;
        }

        public void CountGameObjects(int value) {
            _gameStatistics.NrGameObjects = value;
        }

        public void Log(string value) {
            _logger.Debug.Log(value);
        }
    }
}
