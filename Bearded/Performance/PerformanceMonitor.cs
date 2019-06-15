using System;
using System.Diagnostics;

namespace Bearded.Photones.Performance {
    class PerformanceMonitor {

        private int _frameNrMod60 = 0;

        // FPS
        private const double FPS_SMOOTHING_FACTOR = 0.05;
        private double _fps = 0;
        private double _fpsSmoothed = 60;
        private double _fpsDevSmoothed = 10;
        private double _fpsCurrentMax = 0;
        private double _fpsCurrentMin = 100;
        private double _fpsPreviousMax = 0;
        private double _fpsPreviousMin = 100;

        // Frametime
        private const double FRAMETIME_SMOOTHING_FACTOR = 0.01;
        private double _frametime = 0;
        private double _frametimeSmoothed = 3000;
        private double _frametimeDevSmoothed = 1000;
        private double _frametimeCurrentMax = 0;
        private double _frametimeCurrentMin = 10000;
        private double _frametimePreviousMax = 0;
        private double _frametimePreviousMin = 0;
        // Elapsed time
        private double _elapsed = 0;
        private double _elapsedCurrentMax = 0;
        private double _elapsedCurrentMin = 10000;
        private double _elapsedPreviousMax = 0;
        private double _elapsedPreviousMin = 0;

        private Stopwatch _watch;

        public PerformanceStats GetStats() {
            return new PerformanceStats(
                    _fps,
                    _fpsSmoothed,
                    _fpsDevSmoothed,
                    _fpsPreviousMax,
                    _fpsPreviousMin,
                    _frametime,
                    _frametimeSmoothed,
                    _frametimeDevSmoothed,
                    _frametimePreviousMax,
                    _frametimePreviousMin,
                    _elapsedPreviousMax,
                    _elapsedPreviousMin
                );
        }

        public void StartFrame(double elapsedTimeInS) {

            if (_frameNrMod60 >= 600) {
                _frameNrMod60 = 0;
            }

            CalculateFpsStats(elapsedTimeInS);
            CalculateElapsedStats(elapsedTimeInS);

            _watch = Stopwatch.StartNew();
        }

        public void EndFrame() {
            if (_watch == null) {
                return;
            }

            _watch.Stop();

            CalculateFrametimeStats(_watch);

            _frameNrMod60++;
        }

        private void CalculateElapsedStats(double elapsedTimeInS) {
            _elapsed = elapsedTimeInS * 1000000.0;

            _elapsedCurrentMax = Math.Max(_elapsed, _elapsedCurrentMax);
            _elapsedCurrentMin = Math.Min(_elapsed, _elapsedCurrentMin);

            if (_frameNrMod60 == 0) {
                _elapsedPreviousMax = _elapsedCurrentMax;
                _elapsedPreviousMin = _elapsedCurrentMin;
                _elapsedCurrentMax = 0;
                _elapsedCurrentMin = double.MaxValue;
            }
        }

        private void CalculateFpsStats(double elapsedTimeInS) {
            _fps = 1 / elapsedTimeInS;
            _fpsSmoothed = FPS_SMOOTHING_FACTOR * _fps + (1 - FPS_SMOOTHING_FACTOR) * _fpsSmoothed;
            var fpsDev = Math.Abs(_fpsSmoothed - _fps);
            _fpsDevSmoothed = FPS_SMOOTHING_FACTOR * fpsDev + (1 - FPS_SMOOTHING_FACTOR) * _fpsDevSmoothed;

            _fpsCurrentMax = Math.Max(_fps, _fpsCurrentMax);
            _fpsCurrentMin = Math.Min(_fps, _fpsCurrentMin);

            if (_frameNrMod60 == 0) {
                _fpsPreviousMax = _fpsCurrentMax;
                _fpsPreviousMin = _fpsCurrentMin;
                _fpsCurrentMax = 0;
                _fpsCurrentMin = double.MaxValue;
            }
        }

        private void CalculateFrametimeStats(Stopwatch watch) {
            // frametime is in microseconds
            _frametime = watch.Elapsed.Ticks / 10.0;
            _frametimeSmoothed = FRAMETIME_SMOOTHING_FACTOR * _frametime + (1 - FRAMETIME_SMOOTHING_FACTOR) * _frametimeSmoothed;
            var frametimeDev = Math.Abs(_frametimeSmoothed - _frametime);
            _frametimeDevSmoothed = FRAMETIME_SMOOTHING_FACTOR * frametimeDev + (1 - FRAMETIME_SMOOTHING_FACTOR) * _frametimeDevSmoothed;

            _frametimeCurrentMax = Math.Max(_frametime, _frametimeCurrentMax);
            _frametimeCurrentMin = Math.Min(_frametime, _frametimeCurrentMin);

            if (_frameNrMod60 == 0) {
                _frametimePreviousMax = _frametimeCurrentMax;
                _frametimePreviousMin = _frametimeCurrentMin;
                _frametimeCurrentMax = 0;
                _frametimeCurrentMin = double.MaxValue;
            }

        }
    }
}
