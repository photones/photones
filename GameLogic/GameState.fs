namespace GameLogic

open System.Collections.Generic
open Bearded.Utilities.SpaceTime
open amulware.Graphics

type GameState(gameObjects : List<GameObject<GameState>>) = 

    let tileMap = TileMap(Unit(-1.0f), Unit(-1.0f), Unit(2.0f), Unit(2.0f), 200, 200)

    let mutable _gameObjects = gameObjects

    member this.TileMap = tileMap
    member this.GameObjects = _gameObjects

    member this.Update(uea: UpdateEventArgs): unit =
        // filter alive objects
        _gameObjects <- List(_gameObjects |> List.ofSeq |> List.filter (fun o -> o.Alive))
        // Collection can be modified during update
        let tmpGameObjects = List(_gameObjects)
        for o in tmpGameObjects do
            o.Update this uea
        for o in _gameObjects do
            o.Refresh()

        tileMap.Update(_gameObjects)

    member this.Spawn (obj : GameObject<GameState>) =
        _gameObjects.Add(obj)
