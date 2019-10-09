namespace GameLogic

module GameStateFactory =
    open Bearded.Utilities.SpaceTime
    open System.Collections.Generic

    let BuildInitialGameState() =
        let planet1 = Planet.CreatePlanet ({
            Position = Position2(-1.0f, 0.0f);
            Size = Unit 0.1f;
            PlayerIndex = 0uy;
        })
        let planet2 = Planet.CreatePlanet ({
            Position = Position2(1.0f, 0.0f);
            Size = Unit 0.1f;
            PlayerIndex = 1uy;
        })
        GameState(List<GameObject<GameState>>([planet1;planet2]))

