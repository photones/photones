namespace GameLogic

open System.Collections.Generic
open System.Linq
open Bearded.Utilities.SpaceTime
open GameLogic.Utils


type GameState
    (
    gameParameters: GameParameters.T,
    players:IEnumerable<UpdatableState<PlayerData,GameState>>,
    gameObjects:IEnumerable<GameObject<GameState>>
    ) = 

    let tileMap = TileMap(Unit(-2.0f), Unit(-2.0f), Unit(4.0f), Unit(4.0f), 400, 400)

    let mutable _gameObjects = gameObjects.ToList()
    let mutable _deadGameObjects = List<GameObject<GameState>>()
    let mutable _gameParameters = gameParameters
    let _players = players.ToList()

    member this.GameParameters = _gameParameters
    member this.TileMap = tileMap
    member this.GameObjects = readonly _gameObjects
    member this.DeadGameObjects = readonly _deadGameObjects
    member this.Players = readonly _players

    member this.SetGameParameters(gameParameters: GameParameters.T): unit =
        _gameParameters <- gameParameters

    member this.Update(elapsedS: TimeSpan): unit =
        tileMap.Update(_gameObjects)

        _deadGameObjects <- List(_gameObjects |> Seq.filter (fun o -> not o.Alive))
        _gameObjects <- List(_gameObjects |> Seq.filter (fun o -> o.Alive))

        for p in _players do
            p.Update this elapsedS
        for p in _players do
            p.Refresh()

        for o in _gameObjects do
            match o with
            | Photon d ->
                let neighbors = this.TileMap.GetObjects d.State.Position (Unit 0.00000001f)
                //if neighbors.Count() > 1 then Tracer.Log("Two photons are very close")
                for n in neighbors do
                    match n with
                    | Photon nd ->
                        if nd.State.Position = d.State.Position && nd <> d then Tracer.Log("Two photons have the same position")
                    | _ -> ()
            | _ -> ()

        // Collection can be modified during update, so create a list for iteration
        for o in List(_gameObjects) do
            o.Update this elapsedS
        for o in _gameObjects do
            o.Refresh()

        Utils.Tracer.CountGameObjects (_gameObjects.Count)

    member this.Spawn (obj:GameObject<GameState>) =
        _gameObjects.Add(obj)
