using GameLogic;
using Bearded.Utilities.SpaceTime;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Bearded.Photones.GameUI
{
    class GameStateFactory
    {
        public static GameState BuildInitialGameState() {
            var planets = new Planet[] {
                new Planet(new Position2(.5f, .5f)),
                new Planet(new Position2(-.5f, .5f)),
                new Planet(new Position2(.5f, -.5f)),
                new Planet(new Position2(-.5f, -.5f))
            };

            var photons = RandomPhotons(100).ToArray();

            return new GameState(planets, photons);
        }

        public static IEnumerable<Photon> RandomPhotons(int amount) {
            Random rnd = new Random();
            for (int i = 0; i < amount; i++) {
                float d1 = (float)rnd.NextDouble();
                float d2 = (float)rnd.NextDouble();
                float d3 = (float)rnd.NextDouble();
                float d4 = (float)rnd.NextDouble();
                yield return new Photon(new Position2(d1, d2), new Position2(d3, d4));
            }
        }
    }
}
