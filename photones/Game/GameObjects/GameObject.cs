using System;
using Bearded.Photones.Rendering;
using Bearded.Utilities.Math;
using Bearded.Utilities.SpaceTime;

namespace Bearded.Photones.Game.GameObjects
{
    abstract class GameObject
    {
        public Position2 Position { get; set; }
        public Direction2 Direction { get; set; }

        protected GameObject() { }

        public abstract void Update(Bearded.Utilities.SpaceTime.TimeSpan elapsedTime);

        public abstract void Draw(GeometryManager geometries);
    }
}
