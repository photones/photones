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
            Color = Color.LightSeaGreen;
            Target = Position2.Zero
        })
        let player4 = Player.createPlayer({
            Id = 3uy;
            Color = Color.OrangeRed;
            Target = Position2.Zero
        })
        let player5 = Player.createPlayer({
            Id = 4uy;
            Color = Color.BlueViolet;
            Target = Position2.Zero
        })
        let player6 = Player.createPlayer({
            Id = 5uy;
            Color = Color.GreenYellow;
            Target = Position2.Zero
        })
        let player7 = Player.createPlayer({
            Id = 6uy;
            Color = Color.MediumVioletRed;
            Target = Position2.Zero
        })
        let player8 = Player.createPlayer({
            Id = 7uy;
            Color = Color.CornflowerBlue;
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
        let planet5 = Planet.createPlanet ({
            Position = Position2(0.5f, 0.5f);
            Size = Unit 0.05f;
            Alive = true;
            PlayerId = 4uy;
        })
        let planet6 = Planet.createPlanet ({
            Position = Position2(-0.5f, -0.5f);
            Size = Unit 0.05f;
            Alive = true;
            PlayerId = 5uy;
        })
        let planet7 = Planet.createPlanet ({
            Position = Position2(-0.5f, 0.5f);
            Size = Unit 0.05f;
            Alive = true;
            PlayerId = 6uy;
        })
        let planet8 = Planet.createPlanet ({
            Position = Position2(0.5f, -0.5f);
            Size = Unit 0.05f;
            Alive = true;
            PlayerId = 7uy;
        })
        
        let players = List<Player.T>([player1;player2;player3;player4;player5;player6;player7;player8])
        let planets = List<GameObject<GameState>>([planet1;planet2;planet3;planet4;planet5;planet6;planet7;planet8])
        GameState(players, planets)

