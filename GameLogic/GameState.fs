namespace GameLogic

open System.Collections.Generic
open Bearded.Utilities.SpaceTime

type GameState(gameObjects:List<GameObject<GameState>>) = 

    let tileMap = TileMap(Unit(-1.5f), Unit(-1.0f), Unit(3.0f), Unit(2.0f), 200, 300)

    let mutable _gameObjects = gameObjects
    let mutable _deadGameObjects = List<GameObject<GameState>>()

    member this.TileMap = tileMap
    member this.GameObjects = _gameObjects
    member this.DeadGameObjects = _deadGameObjects

    member this.Update(tracer: Tracer, elapsedS: TimeSpan): unit =
        tileMap.Update(tracer, _gameObjects)

        _deadGameObjects <- List(_gameObjects |> Seq.filter (fun o -> not o.Alive))
        _gameObjects <- List(_gameObjects |> Seq.filter (fun o -> o.Alive))
        // Collection can be modified during update, so create a list for iteration
        for o in List(_gameObjects) do
            o.Update tracer this elapsedS
        for o in _gameObjects do
            o.Refresh()

        tracer.CountGameObjects (_gameObjects.Count)

    member this.Spawn (obj:GameObject<GameState>) =
        _gameObjects.Add(obj)
