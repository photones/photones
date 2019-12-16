namespace GameLogic

open System
open Bearded.Utilities.SpaceTime
open amulware.Graphics
open Utils

module public Player =

    type T = UpdatableState<PlayerData, GameState>

    let allPlanets (gameState:GameState) = [
        for o in gameState.GameObjects do
            match o with
            | Planet d -> yield d.State
            | Photon _ -> ()
    ]

    let targetPeriodInSeconds = 30.0
    let private staticTarget () =
        let now = System.DateTime.Now
        let seconds = float now.Second + (float now.Millisecond / 1000.0)
        let radians = seconds / 60.0 * System.Math.PI * 2.0
        let scaledRadians = radians * (60.0 / targetPeriodInSeconds)
        let x = System.Math.Cos(2.0 * scaledRadians)
        let y = System.Math.Sin(3.0 * scaledRadians)
        new Position2(single x, single y)

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

    let rec update (this:T) (gameState:GameState) (elapsedS:TimeSpan) = 
        let state = this.State
        let target = staticTarget ()

        {
            Id = state.Id;
            Color = state.Color;
            Target = target;
        }

    let createPlayer (data: PlayerData) =
        T(data, update)

    let getPlayerById (gameState:GameState) (id:byte) =
        gameState.Players |> Seq.find (fun p -> p.State.Id = id)
