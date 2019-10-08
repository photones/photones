namespace Bearded.Photones.Particles.Behaviors.LerpLifetime {
    class LerpFloatBehavior : IParticleBehavior<float> {
        float start, end;

        public LerpFloatBehavior(float start, float end) {
            this.start = start;
            this.end = end;
        }

        public float Calculate(Particle particle, float lifetime) {
            return (1 - lifetime) * start + lifetime * end;
        }
    }
}
