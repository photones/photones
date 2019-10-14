using Bearded.Photones.Particles;
using Bearded.Photones.Particles.Behaviors.LerpLifetime;
using Bearded.Photones.Particles.Behaviors;
using GameLogic;
using Bearded.Utilities.SpaceTime;
using amulware.Graphics;

namespace Bearded.Photones.GameUI {
    public static class ExplosionManager {
        public static void Explode(GameObject<GameState> gameObject) {
            gameObject.Visit(ExplodePhoton, ExplodePlanet);
        }

        private static void ExplodePhoton(PhotonData photon) {
            var particle = new Particle();
            particle.AlphaBehavior = new LerpFloatBehavior(1, 0);
            particle.ColorBehavior = new ConstantBehavior<Color>(Color.WhiteSmoke);
            particle.SizeBehavior = new LerpVector2Behavior(photon.Size.NumericValue * 2, 0);
            particle.Lifetime = new TimeSpan(0.8);
            particle.Position = photon.Position;
            ParticleSystem.Get.Add(particle);
        }

        private static void ExplodePlanet(PlanetData planet) {
            var particle = new Particle();
            particle.AlphaBehavior = new LerpFloatBehavior(1, 0);
            particle.ColorBehavior = new ConstantBehavior<Color>(Color.WhiteSmoke);
            particle.SizeBehavior = new LerpVector2Behavior(planet.Size.NumericValue * 2, 0);
            particle.Lifetime = new TimeSpan(0.8);
            particle.Position = planet.Position;
            ParticleSystem.Get.Add(particle);
        }
    }
}
