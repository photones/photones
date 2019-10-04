﻿using Bearded.Photones.Rendering;
using GameLogic;
using OpenTK;
using TimeSpan = Bearded.Utilities.SpaceTime.TimeSpan;
using amulware.Graphics;
using Bearded.Photones.Particles;
using Bearded.Photones.Particles.Behaviors.LerpLifetime;
using Bearded.Photones.Particles.Behaviors;
using Bearded.Utilities.SpaceTime;

namespace Bearded.Photones.GameUI {

    class Beardgame {
        private readonly GameState _gameState;

        public Beardgame() {
            _gameState = GameStateFactory.BuildInitialGameState();
        }

        public void Update(Tracer tracer, UpdateEventArgs updateArgs) {
            _gameState.Update(tracer, updateArgs);
        }

        public void Draw(GeometryManager geometries) {
            // Coord system
            var coordcolor = Color.Red;
            geometries.PhotonGeometry.DrawParticle(new Vector2(0, 0), coordcolor);
            geometries.PhotonGeometry.DrawParticle(new Vector2(1, 1), coordcolor);
            geometries.PhotonGeometry.DrawParticle(new Vector2(1, -1), coordcolor);
            geometries.PhotonGeometry.DrawParticle(new Vector2(-1, 1), coordcolor);
            geometries.PhotonGeometry.DrawParticle(new Vector2(-1, -1), coordcolor);

            var renderer = new GameObjectRenderer(geometries);
            foreach (var gameObject in _gameState.GameObjects) {
                renderer.Render(gameObject);
            }
            foreach (var gameObject in _gameState.DeadGameObjects) {
                var particle = new Particle();
                particle.AlphaBehavior = new LerpFloatBehavior(1, 0);
                particle.ColorBehavior = new ConstantBehavior<Color>(Color.Red);
                particle.SizeBehavior = new LerpVector2Behavior(0.01f, 0);
                particle.Lifetime = new TimeSpan(1);
                particle.Position = gameObject.Position;
                ParticleSystem.Get.Add(particle);
            }
        }
    }
}
