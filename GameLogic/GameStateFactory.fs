namespace GameLogic

module GameStateFactory =
    open Bearded.Utilities
    open Bearded.Utilities.SpaceTime
    open Bearded.Utilities.Geometry
    open System.Collections.Generic
    open amulware.Graphics
    open System.Linq

    let emptyGameState() =
        GameState(List<Player.T>(), List<GameObject<GameState>>())

    let playerColors = [
        Color.Goldenrod;
        Color.HotPink;
        Color.LightSeaGreen;
        Color.OrangeRed;
        Color.BlueViolet;
        Color.ForestGreen;
        Color(0xFFAB2222u); // Pale red
        Color.CornflowerBlue;
    ]

    let addPlayer (gameState:GameState) =
        let playerCount = gameState.Players.Count
        let newPlayerId = byte playerCount
        let newPlayerColor = playerColors.[int newPlayerId]
        let newPlayer = Player.createPlayer({
            Id = newPlayerId;
            Target = Position2.Zero;
            Color = newPlayerColor;
        })
        let players = gameState.Players.ToList()
        players.Add(newPlayer)
        GameState(players, gameState.GameObjects), newPlayerId

    let addPlanet (position:Position2) playerId (gameState:GameState) =
        let newPlanet = Planet.createPlanet ({
            Position = position;
            Size = Unit 0.05f;
            Alive = true;
            PlayerId = playerId;
        })
        let gameObjects = gameState.GameObjects.ToList()
        gameObjects.Add(newPlanet)
        GameState(gameState.Players, gameObjects)


    /// Let all players start with one planet on a circle
    let defaultScenario () =
        let playerCount = 8
        let radius = Unit 0.8f
        let angle = Angle.FromRadians(Mathf.Tau / single playerCount)
        let addPlayerWithPlanet (gameState, dir) _ =
            let planetPos = Position2.Zero + Difference2.In(dir, radius)
            let newGameState, newPlayerId = addPlayer gameState
            addPlanet planetPos newPlayerId newGameState, dir + angle
        let start = emptyGameState (), Direction2.Zero
        Seq.fold addPlayerWithPlanet start [1..playerCount] |> fst
        
