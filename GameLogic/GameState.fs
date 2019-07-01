namespace GameLogic

open System.Collections.Generic
open Bearded.Utilities.SpaceTime

type GameState(gameObjects : List<GameObject>) = 

    let tileMap = TileMap(Unit(-1.0f), Unit(-1.0f), Unit(2.0f), Unit(2.0f), 200, 200)

    let mutable _gameObjects = gameObjects

    member this.TileMap = tileMap
    member this.GameObjects = _gameObjects

    member this.Update(elapsedTime: TimeSpan, totalTime: TimeSpan): unit =
        // filter alive objects
        _gameObjects <- List(_gameObjects |> List.ofSeq |> List.filter (fun o -> o.Alive))
        // Collection can be modified during update
        let tmpGameObjects = List(_gameObjects)
        for o in tmpGameObjects do
            o.Update this elapsedTime totalTime
        for o in _gameObjects do
            o.Refresh()

        tileMap.Update(Seq.cast _gameObjects)

    interface IGameState with
        member this.SpawnPhoton (state : PhotonState) =
            _gameObjects.Add(Photon (UpdatableState(state,Photon.update)))

        member this.GameObjects = _gameObjects |> Seq.map (fun x -> x :> IGameObject)
        member this.TileMap = upcast this.TileMap

        member this.Update (elapsedTime: TimeSpan) (totalTime: TimeSpan): unit =
            this.Update(elapsedTime, totalTime)

