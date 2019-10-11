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

    let rec Update (tracer:Tracer) (this:T) (gameState:GameState) (elapsedS:TimeSpan) = 
        let state = this.State
        let planets = allPlanets gameState
        let hostilePlanets = planets |> List.filter (fun s -> s.PlayerId <> state.Id)
        let target =
            if Seq.isEmpty hostilePlanets
            then Position2.Zero
            else
                let index = randomInt (hostilePlanets.Length)
                let randomHostilePlanet = hostilePlanets.[index]
                randomHostilePlanet.Position

        {
            Id = state.Id;
            Color = state.Color;
            Target = target;
        }

    let public CreatePlayer (data: PlayerData) =
        T(data, Update)

    let getPlayerById (gameState:GameState) (id:byte) =
        gameState.Players |> Seq.find (fun p -> p.State.Id = id)
