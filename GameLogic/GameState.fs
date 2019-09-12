namespace GameLogic

open System.Collections.Generic
open Bearded.Utilities.SpaceTime
open amulware.Graphics
open Utils

type GameState(gameObjects : List<GameObject<GameState>>) = 

    let tileMap = TileMap(Unit(-1.0f), Unit(-1.0f), Unit(2.0f), Unit(2.0f), 200, 200)

    let mutable _gameObjects = gameObjects

    member this.TileMap = tileMap
    member this.GameObjects = _gameObjects

    member this.Update(tracer: Tracer, updateArgs: UpdateEventArgs): unit =
        tileMap.Update(_gameObjects)

        _gameObjects <- List(_gameObjects |> Seq.filter (fun o -> o.Alive))
        // Collection can be modified during update, so create a list for iteration
        for o in List(_gameObjects) do
            o.Update tracer this updateArgs
        for o in _gameObjects do
            o.Refresh()

        tracer.CountGameObjects (_gameObjects.Count)

    member this.Spawn (obj : GameObject<GameState>) =
        _gameObjects.Add(obj)
