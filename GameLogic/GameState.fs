namespace GameLogic

open System.Collections.Generic
open Bearded.Utilities.SpaceTime
open GameLogic.Utils


type GameState
     (
     players:List<UpdatableState<PlayerData,GameState>>,
     gameObjects:List<GameObject<GameState>>
     ) = 

    let tileMap = TileMap(Unit(-2.0f), Unit(-2.0f), Unit(4.0f), Unit(4.0f), 400, 400)

    let mutable _gameObjects = gameObjects
    let mutable _deadGameObjects = List<GameObject<GameState>>()
    let _players = players

    member this.TileMap = tileMap
    member this.GameObjects = readonly _gameObjects
    member this.DeadGameObjects = readonly _deadGameObjects
    member this.Players = readonly _players

    member this.Update(elapsedS: TimeSpan): unit =
        tileMap.Update(_gameObjects)

        _deadGameObjects <- List(_gameObjects |> Seq.filter (fun o -> not o.Alive))
        _gameObjects <- List(_gameObjects |> Seq.filter (fun o -> o.Alive))

        for p in _players do
            p.Update this elapsedS
        for p in _players do
            p.Refresh()

        // Collection can be modified during update, so create a list for iteration
        for o in List(_gameObjects) do
            o.Update this elapsedS
        for o in _gameObjects do
            o.Refresh()

        Utils.Tracer.CountGameObjects (_gameObjects.Count)

    member this.Spawn (obj:GameObject<GameState>) =
        _gameObjects.Add(obj)
