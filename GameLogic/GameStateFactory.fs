namespace GameLogic

module GameStateFactory =
    open Bearded.Utilities.SpaceTime
    open System.Collections.Generic
    open amulware.Graphics

    let BuildInitialGameState() =
        let player1 = Player(0uy, Color.Goldenrod) :> IUpdatetablePlayer<GameState>
        let player2 = Player(1uy, Color.HotPink) :> IUpdatetablePlayer<GameState>
        let planet1 = Planet.CreatePlanet ({
            Position = Position2(-1.0f, 0.0f);
            Size = Unit 0.1f;
            Alive = true;
            Player = player1;
        })
        let planet2 = Planet.CreatePlanet ({
            Position = Position2(1.0f, 0.0f);
            Size = Unit 0.1f;
            Alive = true;
            Player = player2;
        })
        GameState(List<IUpdatetablePlayer<GameState>>([player1;player2]), List<GameObject<GameState>>([planet1;planet2]))

