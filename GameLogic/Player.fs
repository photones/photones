﻿namespace GameLogic

open System
open Bearded.Utilities.SpaceTime
open Bearded.Graphics
open Utils

module public Player =

    type T = UpdatableState<PlayerData, GameState>

    let allPlanets (gameState:GameState) = [
        for o in gameState.GameObjects do
            match o with
            | Planet d -> yield d.State
            | Photon _ -> ()
    ]

    let rec update (this:T) (gameState:GameState) (elapsedS:TimeSpan) = 
        let state = this.State
        let planets = allPlanets gameState
        let hostilePlanets = planets |> List.filter (fun s -> s.PlayerId <> state.Id)
        let target =
            if Seq.isEmpty hostilePlanets
            then Position2.Zero
            else
                // Just some deterministic but arbitrary choice
                let index = (int state.Id) % (hostilePlanets.Length)
                let randomHostilePlanet = hostilePlanets.[index]
                randomHostilePlanet.Position

        {
            Id = state.Id;
            Color = state.Color;
            Target = target;
        }

    let createPlayer (data: PlayerData) =
        T(data, update)

    let getPlayerById (gameState:GameState) (id:byte) =
        gameState.Players |> Seq.find (fun p -> p.State.Id = id)
