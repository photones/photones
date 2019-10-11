namespace GameLogic

module GameStateFactory =
    open Bearded.Utilities.SpaceTime
    open System.Collections.Generic
    open amulware.Graphics

    let BuildInitialGameState() =
        let player1 = Player.createPlayer({
            Id = 0uy;
            Color = Color.Goldenrod;
            Target = Position2.Zero
        })
        let player2 = Player.createPlayer({
            Id = 1uy;
            Color = Color.HotPink;
            Target = Position2.Zero
        })
        let player3 = Player.createPlayer({
            Id = 2uy;
            Color = Color.Green;
            Target = Position2.Zero
        })
        let player4 = Player.createPlayer({
            Id = 3uy;
            Color = Color.Red;
            Target = Position2.Zero
        })
        let planet1 = Planet.createPlanet ({
            Position = Position2(-1.0f, 0.0f);
            Size = Unit 0.05f;
            Alive = true;
            PlayerId = 0uy;
        })
        let planet2 = Planet.createPlanet ({
            Position = Position2(1.0f, 0.0f);
            Size = Unit 0.05f;
            Alive = true;
            PlayerId = 1uy;
        })
        let planet3 = Planet.createPlanet ({
            Position = Position2(0.0f, 0.8f);
            Size = Unit 0.05f;
            Alive = true;
            PlayerId = 2uy;
        })
        let planet4 = Planet.createPlanet ({
            Position = Position2(0.0f, -0.8f);
            Size = Unit 0.05f;
            Alive = true;
            PlayerId = 3uy;
        })
        let players = List<Player.T>([player1;player2;player3;player4])
        let planets = List<GameObject<GameState>>([planet1;planet2;planet3;planet4])
        GameState(players, planets)

