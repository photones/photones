namespace GameLogic

open System.Collections.Generic
open Bearded.Utilities.SpaceTime

type GameState(gameObjects : List<GameObject>) = 

    let tileMap = new TileMap(new Unit(-1.0f), new Unit(-1.0f), new Unit(2.0f), new Unit(2.0f), 200, 200)

    member this.TileMap = tileMap
    member this.GameObjects = gameObjects

    member this.Update(elapsedTime: TimeSpan): unit =
        for o in gameObjects do
            o.Update this elapsedTime
        for o in gameObjects do
            o.Refresh()

        tileMap.Update(gameObjects)

    interface IGameState with

        member this.GameObjects = gameObjects |> Seq.cast<IGameObject>
        member this.TileMap = upcast this.TileMap

        member this.Update(elapsedTime: TimeSpan): unit =
            this.Update(elapsedTime)

