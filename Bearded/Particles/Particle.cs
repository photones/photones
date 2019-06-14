using amulware.Graphics;
using Bearded.Photones.Rendering;
using Bearded.Photones.Particles.Behaviors;
using Bearded.Utilities;
using Bearded.Utilities.SpaceTime;
using OpenTK;
using TimeSpan = Bearded.Utilities.SpaceTime.TimeSpan;

namespace Bearded.Photones.Particles {
    class Particle {
        public const float WIDTH = 24;
        public const float HEIGHT = 24;

        public Position2 Position { get; set; }
        public Velocity2 Velocity { get; set; }
        /// <summary>
        /// Lifetime in seconds
        /// </summary>
        public TimeSpan Lifetime { get; set; }
        public TimeSpan InitialLifetime { get; private set; }

        public IParticleBehavior<Vector2> SizeBehavior { get; set; }
        public IParticleBehavior<Color> ColorBehavior { get; set; }
        public IParticleBehavior<float> AlphaBehavior { get; set; }

        public bool IsAlive => Lifetime.NumericValue > 0;

        public void Kill() => Lifetime = TimeSpan.Zero;

        public void Update(TimeSpan elapsedTimeInS) {
            if (InitialLifetime == TimeSpan.Zero) {
                firstRun();
            }

            Position += elapsedTimeInS * Velocity;
            Lifetime -= elapsedTimeInS;
        }

        private void firstRun() {
            InitialLifetime = Lifetime;
        }

        public void Draw(GeometryManager geometries) {
            float lifetime = 1 - (float)(Lifetime / InitialLifetime);

            geometries.SpriteGeometry.Size = SizeBehavior.Calculate(this, lifetime);
            geometries.SpriteGeometry.Color = new Color(ColorBehavior.Calculate(this, lifetime), (byte)(255 * AlphaBehavior.Calculate(this, lifetime)));
            geometries.SpriteGeometry.DrawSprite(Position.NumericValue);
        }
    }
}
