namespace Bearded.Photones.Particles.Behaviors
{
    interface IParticleBehavior<T>
    {
        T Calculate(Particle particle, float lifetime);
    }
}
