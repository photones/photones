using System;
using System.Globalization;
using System.Runtime;
using System.Threading;
using System.Threading.Tasks;
using amulware.Graphics;
using Bearded.Photones.GameUI;
using Bearded.Utilities.Input;
using Bearded.Photones.Rendering;
using Bearded.Photones.Screens;
using Bearded.Utilities.IO;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using Bearded.Photones.Performance;
using GameLogic;
using Bearded.Photones.Utilities;

namespace Bearded.Photones {
    public class PhotonesProgram : Program {
        static void Main(string[] args) {
            using (Toolkit.Init(new ToolkitOptions() { Backend = PlatformBackend.PreferNative })) {
                Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

                var logger = new Logger();

                logger.Info.Log("");
                logger.Info.Log("Creating game");
                var game = new PhotonesProgram(GameStateFactory.defaultScenario(2));
                Utils.Tracer = new Tracer(logger, game._gameStatistics);

                logger.Info.Log("Running game");
                game.Run();

                logger.Info.Log("Safely exited game");
            }
        }

        // NOTE: This is probably a sign that I don't use the ViewportSize class like I should.
        public const float WIDTH = 1280;
        public const float HEIGHT = 720;

        public const int MAJOR = 0;
        public const int MINOR = 0;

        private InputManager _inputManager;
        private RenderContext _renderContext;
        private ScreenManager _screenManager;
        private readonly PerformanceMonitor _performanceMonitor;
        private readonly GameStatistics _gameStatistics;
        private readonly Action<PhotonesProgram, BeardedUpdateEventArgs> _afterFrame;
        private readonly GameState _gameState;

        public PhotonesProgram(
                GameState initialGameState,
                Action<PhotonesProgram, BeardedUpdateEventArgs> afterFrame = null
            )
            : base((int)WIDTH, (int)HEIGHT, GraphicsMode.Default, "photones",
                GameWindowFlags.Default, DisplayDevice.Default, MAJOR, MINOR,
                GraphicsContextFlags.Default) {
            Console.WriteLine(DisplayDevice.Default.ToString());
            Console.WriteLine(GL.GetString(StringName.Renderer));
            Console.WriteLine(GL.GetString(StringName.Version));
            _performanceMonitor = new PerformanceMonitor();
            _gameStatistics = new GameStatistics();
            _afterFrame = afterFrame;
            _gameState = initialGameState;
        }

        protected override void OnLoad(EventArgs e) {
            _renderContext = new RenderContext();

            _inputManager = new InputManager(this);

            _screenManager = new ScreenManager(_inputManager);
            _screenManager.AddScreenLayerOnTop(new GameScreen(_screenManager,
                _renderContext.Geometries, _gameState));
            _screenManager.AddScreenLayerOnTop(new HudScreen(_screenManager,
                _renderContext.Geometries));

            KeyPress += (sender, args) => _screenManager.RegisterPressedCharacter(args.KeyChar);

            OnResize(EventArgs.Empty);
        }

        protected override void OnResize(EventArgs e) {
            _screenManager.OnResize(new ViewportSize(Width, Height));
        }

        protected override void OnUpdateUIThread() {
            _inputManager.ProcessEventsAsync();
        }

        protected override void OnUpdate(UpdateEventArgs updateArgs) {
            if (updateArgs.ElapsedTimeInS < 0.0001) {
                // This is basically to skip the first frame, because its
                // elapsed time is out of whack. We do a GC though, to clean up
                // initialization garbage, to prevent that from happening in
                // game This saves one big stutter a few seconds into the game.
                GC.Collect();
                return;
            }


            var performanceSummary = new PerformanceSummary(
                _performanceMonitor.FrameTime.Stats,
                _performanceMonitor.ElapsedTime.Stats,
                _performanceMonitor.FrameTime.CurrentValue,
                _gameStatistics.NrGameObjects);
            var e = new BeardedUpdateEventArgs(updateArgs, performanceSummary);

            _performanceMonitor.StartFrame(e.UpdateEventArgs.ElapsedTimeInS);

            _inputManager.Update(Focused);
            if (_inputManager.IsKeyPressed(Key.AltLeft) && _inputManager.IsKeyPressed(Key.F4)) {
                Close();
            }

            _screenManager.Update(e);

            _performanceMonitor.EndFrame();
            _afterFrame?.Invoke(this, e);
        }

        protected override void OnRender(UpdateEventArgs e) {
            _renderContext.Compositor.PrepareForFrame();
            _screenManager.Render(_renderContext);
            _renderContext.Compositor.FinalizeFrame();

            SwapBuffers();
        }
    }
}
