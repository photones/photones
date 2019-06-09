using System;
using System.Globalization;
using System.Threading;
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

namespace Bearded.Photones {
    class PhotonesProgram : Program {
        static void Main(string[] args) {
            using (Toolkit.Init(new ToolkitOptions() { Backend = PlatformBackend.PreferNative })) {
                Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

                var logger = new Logger();

                logger.Info.Log("");
                logger.Info.Log("Creating game");
                var game = new PhotonesProgram(logger);

                logger.Info.Log("Running game");
                game.Run(60);

                logger.Info.Log("Safely exited game");
            }
        }

        // NOTE: This is probably a sign that I don't use the ViewportSize class like I should.
        public const float WIDTH = 1280;
        public const float HEIGHT = 720;

        public const int MAJOR = 0;
        public const int MINOR = 0;

        private readonly Logger logger;

        private InputManager _inputManager;
        private RenderContext _renderContext;
        private ScreenManager _screenManager;
        private PerformanceMonitor _performanceMonitor;

        public PhotonesProgram(Logger logger)
            : base((int)WIDTH, (int)HEIGHT, GraphicsMode.Default, "photones",
                GameWindowFlags.Default, DisplayDevice.Default, MAJOR, MINOR, GraphicsContextFlags.Default) {
            Console.WriteLine(DisplayDevice.Default.ToString());
            Console.WriteLine(GL.GetString(StringName.Renderer));
            Console.WriteLine(GL.GetString(StringName.Version));
            this.logger = logger;
            _performanceMonitor = new PerformanceMonitor();
        }

        protected override void OnLoad(EventArgs e) {
            _renderContext = new RenderContext();

            _inputManager = new InputManager(this);

            _screenManager = new ScreenManager(_inputManager);
            _screenManager.AddScreenLayerOnTop(new GameScreen(_screenManager, _renderContext.Geometries));
            _screenManager.AddScreenLayerOnTop(new HudScreen(_screenManager, _renderContext.Geometries));

            KeyPress += (sender, args) => _screenManager.RegisterPressedCharacter(args.KeyChar);

            OnResize(EventArgs.Empty);
        }

        protected override void OnResize(EventArgs e) {
            _screenManager.OnResize(new ViewportSize(Width, Height));
        }

        protected override void OnUpdateUIThread() {
            _inputManager.ProcessEventsAsync();
        }

        protected override void OnUpdate(UpdateEventArgs uea) {
            var e = new BeardedUpdateEventArgs(uea, _performanceMonitor.GetStats());

            _performanceMonitor.StartFrame(e.ElapsedTimeInS);

            _inputManager.Update(Focused);
            if (_inputManager.IsKeyPressed(Key.AltLeft) && _inputManager.IsKeyPressed(Key.F4)) {
                Close();
            }
            _screenManager.Update(e);

            _performanceMonitor.EndFrame();
        }

        protected override void OnRender(UpdateEventArgs e) {
            _renderContext.Compositor.PrepareForFrame();
            _screenManager.Render(_renderContext);
            _renderContext.Compositor.FinalizeFrame();

            SwapBuffers();
        }
    }
}
