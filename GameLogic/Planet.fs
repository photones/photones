namespace GameLogic

open System
open Bearded.Utilities.SpaceTime
open Utils

module public Planet =

    type T = UpdatableState<PlanetData, GameState>

    let spawnRate = 100.0 // # per second

    let private isHostile (state:PlanetData) (other:PhotonData) =
        other.PlayerId <> state.PlayerId

    let private getNeighbors (this:T) (gameState:GameState) (radius:Unit) =
        let neighbors = gameState.TileMap.GetObjects this.State.Position radius
        seq {
            for n in neighbors do
                match n with
                | Planet _ -> ()
                | Photon d -> yield d.State
        }

    let Update (tracer:Tracer) (this:T)
            (gameState:GameState) (elapsedS:TimeSpan):PlanetData = 
        let state = this.State

        // Compute number of spawns.
        // Usually the spawnrate is less than 1. We don't want to hassle with communicating with
        // other frames about how many photons they've already spawned. So we take the stochastic
        // approach.
        let expectedNrOfSpawns = spawnRate * elapsedS.NumericValue
        let certainSpawns = Math.Floor(expectedNrOfSpawns)
        let additionalSpawnChance = expectedNrOfSpawns - certainSpawns
        let additionalSpawnOutcome = bernoulli additionalSpawnChance
        let totalSpawns = (int certainSpawns) + additionalSpawnOutcome

        let behavior = Neutral // if state.Player = 0uy then Aggressive else Shy
        // Spawn photons every frame
        for i = 1 to totalSpawns do
            let photon = Photon.CreatePhoton ({
                    Position = state.Position;
                    Velocity = Velocity2(0.0f, 0.0f);
                    Size = Unit 0.01f;
                    Alive = true;
                    PlayerId = state.PlayerId;
                    Behavior = behavior;
                })
            gameState.Spawn photon

        // Check aliveness
        let mutable alive = true
        let collidingPhotons = getNeighbors this gameState state.Size
        let collidingHostiles = collidingPhotons |> Seq.filter (isHostile state)
        if Seq.isEmpty collidingHostiles |> not then alive <- false

        {
            Position = state.Position;
            Size = state.Size;
            Alive = alive;
            PlayerId = state.PlayerId;
        }

    let public CreatePlanet (data: PlanetData) =
        Planet (T(data, Update))

