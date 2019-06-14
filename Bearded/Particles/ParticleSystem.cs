using System.Collections.Generic;
using Bearded.Photones.Rendering;
using TimeSpan = Bearded.Utilities.SpaceTime.TimeSpan;

namespace Bearded.Photones.Particles {
    class ParticleSystem {
        private static ParticleSystem particleSystem;
        public static ParticleSystem Get => particleSystem ?? (particleSystem = new ParticleSystem());

        private List<Particle> Particles { get; set; }

        private ParticleSystem() {
            Particles = new List<Particle>();
        }

        public void Add(Particle particle) {
            Particles.Add(particle);
        }

        public void Update(TimeSpan elapsedTimeInS) {
            for (int i = Particles.Count - 1; i >= 0; i--) {
                var particle = Particles[i];

                particle.Update(elapsedTimeInS);

                if (!particle.IsAlive) {
                    Particles.RemoveAt(i);
                }
            }
        }

        public void Draw(GeometryManager geometries) {
            foreach (var particle in Particles) {
                particle.Draw(geometries);
            }
        }
    }
}
