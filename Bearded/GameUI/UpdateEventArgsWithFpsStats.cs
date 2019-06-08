using amulware.Graphics;

namespace photones.GameUI {
    class UpdateEventArgsWithFpsStats : UpdateEventArgs {
        public UpdateEventArgsWithFpsStats(UpdateEventArgs e) : base(e.TimeInS) {
            Frame = e.Frame;
        }
    }
}
