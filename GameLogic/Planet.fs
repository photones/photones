namespace GameLogic

open Bearded.Utilities.SpaceTime
open Utils
open amulware.Graphics

module public Planet =

    let Update (tracer : Tracer) (this : UpdatableState<PlanetData, GameState>)
            (gameState : GameState) (updateArgs : UpdateEventArgs) : PlanetData = 
        let state = this.State
        // Spawn photons every frame
        for _ in [1..1] do
            let photon = Photon.CreatePhoton ({
                    Position = state.Position;
                    Velocity = Velocity2(0.0f, 0.0f);
                    Alive = true;
                    PlayerIndex = state.PlayerIndex;
                })
            gameState.Spawn photon

        {
            Position = state.Position;
            PlayerIndex = state.PlayerIndex;
        }

    let public CreatePlanet (data: PlanetData) =
        Planet (UpdatableState<PlanetData, GameState>(data, Update))

