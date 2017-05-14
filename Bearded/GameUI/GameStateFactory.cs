using GameLogic;
using Bearded.Utilities.SpaceTime;

namespace Bearded.Photones.GameUI
{
    class GameStateFactory
    {
        public static GameState BuildDummyGameState() {
            var planets = new Planet[] {
                new Planet(new Position2(.5f, .5f)),
                new Planet(new Position2(-.5f, .5f)),
                new Planet(new Position2(.5f, -.5f)),
                new Planet(new Position2(-.5f, -.5f))
            };

            return new GameState(planets);
        }
    }
}
