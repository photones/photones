namespace GameLogic

open System
open Bearded.Utilities.SpaceTime
open amulware.Graphics
open Utils
open Bearded.Utilities

module public Player =

    type T = UpdatableState<PlayerData, GameState>

    let allPlanets (gameState:GameState) = [
        for o in gameState.GameObjects do
            match o with
            | Planet d -> yield d.State
            | Photon _ -> ()
    ]

    let private playerTarget
            (state:PlayerData) (gameState:GameState) (elapsedS:TimeSpan)
            (inputActions:InputActions.PlayerActions) =
        let xDiff = inputActions.MoveHorizontal.AnalogAmount * 0.9f * single elapsedS.NumericValue
        let yDiff = inputActions.MoveVertical.AnalogAmount * 0.9f * single elapsedS.NumericValue
        let x = MathExtensions.Clamped(state.Target.X.NumericValue + xDiff, -1.0f, 1.0f)
        let y = MathExtensions.Clamped(state.Target.Y.NumericValue + yDiff, -1.0f, 1.0f)
        new Position2(x, y)

    let private attackHostile (state:PlayerData) (gameState:GameState) =
        let planets = allPlanets gameState
        let hostilePlanets = planets |> List.filter (fun s -> s.PlayerId <> state.Id)
        if Seq.isEmpty hostilePlanets
        then Position2.Zero
        else
            // Just some deterministic but arbitrary choice
            let index = (int state.Id) % (hostilePlanets.Length)
            let randomHostilePlanet = hostilePlanets.[index]
            randomHostilePlanet.Position

    let update (this:T) (gameState:GameState) (elapsedS:TimeSpan) (inputActions:InputActions.T) =
        let state = this.State
        let target = playerTarget state gameState elapsedS inputActions.PlayerActions

        {
            Id = state.Id
            Color = state.Color
            Target = target
        }

    let createPlayer (data: PlayerData) =
        T(data, update)

    let getPlayerById (gameState:GameState) (id:byte) =
        gameState.Players |> Seq.find (fun p -> p.State.Id = id)
