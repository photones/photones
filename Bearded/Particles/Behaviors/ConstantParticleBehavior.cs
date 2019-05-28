namespace Bearded.Photones.Particles.Behaviors {
    class ConstantBehavior<T> : IParticleBehavior<T> {
        private T value;

        public ConstantBehavior(T value) => this.value = value;

        public T Calculate(Particle particle, float lifetime) => value;
    }
}
