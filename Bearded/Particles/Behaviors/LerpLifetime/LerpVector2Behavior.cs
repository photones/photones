using OpenTK;

namespace Bearded.Photones.Particles.Behaviors.LerpLifetime {
    class LerpVector2Behavior : IParticleBehavior<Vector2> {
        Vector2 start, end;

        public LerpVector2Behavior(float start, float end)
            : this(new Vector2(start), new Vector2(end)) { }

        public LerpVector2Behavior(Vector2 start, Vector2 end) {
            this.start = start;
            this.end = end;
        }

        public Vector2 Calculate(Particle particle, float lifetime) {
            return new Vector2(
                (1 - lifetime) * start.X + lifetime * end.X,
                (1 - lifetime) * start.Y + lifetime * end.Y);
        }
    }
}
