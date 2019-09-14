namespace GameLogic

open Bearded.Utilities.SpaceTime
open Utils
open amulware.Graphics

module public Planet =

    let Update (tracer : Tracer) (this : PlanetData) (gameState : GameState)
                (updateArgs : UpdateEventArgs) : PlanetData = 
        // Spawn a photon every frame
        let photon = Photon.CreatePhoton ({
                Position = this.Position;
                Speed = Velocity2(0.0f, 0.0f);
                Alive = true;
                PlayerIndex = this.PlayerIndex;
            })
        gameState.Spawn photon

        {
            Position = this.Position;
            PlayerIndex = this.PlayerIndex;
        }

    let public CreatePlanet (data: PlanetData) = Planet (UpdatableState<PlanetData, GameState>(data, Update))

