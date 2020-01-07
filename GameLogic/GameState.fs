namespace GameLogic

open System.Collections.Generic
open System.Linq
open System
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

    member this.Update(elapsedS: TimeSpan, inputActions: InputActions.T): unit =
        this.UpdateGameParameters(inputActions)
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

    member private this.UpdateGameParameters(input: InputActions.T): unit =
        let current = _gameParameters
        let modTimeDiff = input.ModGameSpeed.AnalogAmount * 0.001f
        let modADiff = input.ModA.AnalogAmount * 0.001f
        let modBDiff = input.ModB.AnalogAmount * 0.001f
        let modCDiff = input.ModC.AnalogAmount * 0.001f
        let intModDDiff = if input.IntModD.Hit then int input.IntModD.AnalogAmount else 0
        _gameParameters <- {
            current with
                TimeModifier = Math.Max(0.0, current.TimeModifier + float modTimeDiff)
                ModA = Math.Max(0.0f, current.ModA + modADiff)
                ModB = Math.Max(0.0f, current.ModB + modBDiff)
                ModC = Math.Max(0.0f, current.ModC + modCDiff)
                IntModD = Math.Max(0, current.IntModD + intModDDiff)
        }
