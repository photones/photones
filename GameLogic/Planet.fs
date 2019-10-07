namespace GameLogic

open System
open Bearded.Utilities.SpaceTime
open Utils

module public Planet =

    let spawnRate = 100.0 // # per second

    let Update (tracer:Tracer) (this:UpdatableState<PlanetData, GameState>)
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

        let behavior = if state.PlayerIndex = 0uy then Shy else Neutral
        // Spawn photons every frame
        for i = 1 to totalSpawns do
            let photon = Photon.CreatePhoton ({
                    Position = state.Position;
                    Velocity = Velocity2(0.0f, 0.0f);
                    Alive = true;
                    PlayerIndex = state.PlayerIndex;
                    Behavior = behavior;
                })
            gameState.Spawn photon

        {
            Position = state.Position;
            PlayerIndex = state.PlayerIndex;
        }

    let public CreatePlanet (data: PlanetData) =
        Planet (UpdatableState<PlanetData, GameState>(data, Update))

