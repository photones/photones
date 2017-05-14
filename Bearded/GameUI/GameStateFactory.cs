using GameLogic;
using Bearded.Utilities.SpaceTime;

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

            var photons = new Photon[] {
                new Photon(new Position2(.2f, 0)),
                new Photon(new Position2(.3f, .1f))
            };

            return new GameState(planets, photons);
        }
    }
}
