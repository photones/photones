namespace GameLogic

module GameStateFactory =
    open Bearded.Utilities.SpaceTime
    open System.Collections.Generic
    open amulware.Graphics

    let BuildInitialGameState() =
        let player1 = Player(0uy, Color.Goldenrod) :> IUpdatetablePlayer<GameState>
        let player2 = Player(1uy, Color.HotPink) :> IUpdatetablePlayer<GameState>
        let player3 = Player(2uy, Color.Green) :> IUpdatetablePlayer<GameState>
        let player4 = Player(3uy, Color.Red) :> IUpdatetablePlayer<GameState>
        let planet1 = Planet.CreatePlanet ({
            Position = Position2(-1.0f, 0.0f);
            Size = Unit 0.05f;
            Alive = true;
            Player = player1;
        })
        let planet2 = Planet.CreatePlanet ({
            Position = Position2(1.0f, 0.0f);
            Size = Unit 0.05f;
            Alive = true;
            Player = player2;
        })
        let planet3 = Planet.CreatePlanet ({
            Position = Position2(0.0f, 0.8f);
            Size = Unit 0.05f;
            Alive = true;
            Player = player3;
        })
        let planet4 = Planet.CreatePlanet ({
            Position = Position2(0.0f, -0.8f);
            Size = Unit 0.05f;
            Alive = true;
            Player = player4;
        })
        let players = List<IUpdatetablePlayer<GameState>>([player1;player2;player3;player4])
        let planets = List<GameObject<GameState>>([planet1;planet2;planet3;planet4])
        GameState(players, planets)

