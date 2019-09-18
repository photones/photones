namespace GameLogic

open Bearded.Utilities.SpaceTime
open Utils
open amulware.Graphics
open Microsoft.FSharp.Core
open Microsoft.FSharp.Collections

module public Photon =

    let private interactionRadius = Unit(0.02f)
    let private collisionRadius = Unit(0.003f)

    let private capVelocity maxSpeed (v:Velocity2) =
        if v.Length.NumericValue > maxSpeed
        then Velocity2(v.NumericValue.Normalized() * maxSpeed)
        else v

    let private velocityToGoal (this:PhotonData) (elapsedTime:TimeSpan) (maxSpeed:single) =
        let pointOfAttraction =
            if this.PlayerIndex = 0uy
            then Position2(0.9f, 0.0f)
            else Position2(-0.9f, 0.0f)
        let diff = pointOfAttraction - this.Position
        let acceleration =
            if diff.Length = Unit.Zero
            then Acceleration2.Zero
            // Don't use Direction2 for reasons of performance
            else Acceleration2(diff.NumericValue.Normalized())
        maxSpeed * (acceleration * elapsedTime)

    let private randomSingle2 () = (randomSingle () - 0.5f) * 2.0f
    let private randomVelocity (elapsedTime:TimeSpan) (maxSpeed:single) =
        let acceleration = new Acceleration2(randomSingle2 (), randomSingle2 ())
        maxSpeed * (acceleration * elapsedTime)

    let private accPos = fun total (pos:PhotonData) -> total + (Position2.Zero - pos.Position)
    let private avgPosition (photons: seq<PhotonData>) =
        let sum = Seq.fold accPos Difference2.Zero photons
        let count = Seq.length photons
        sum * (single count) + Position2.Zero

    let filterPhotons objects = seq{
        for n in objects do
            match n with
            | Planet _ -> ()
            | Photon d -> yield d.State
        }

    /// Get avg friendly neighbor position and move away from it
    let private interactionVelocity (this:PhotonData) (gameState:GameState) (elapsedTime:TimeSpan) (maxSpeed:single) = 
        let neighbors = gameState.TileMap.GetNeighbors this.Position interactionRadius
        let friendlyNeighborPhotonData =
            filterPhotons neighbors |>
            Seq.filter (fun data -> data.PlayerIndex = this.PlayerIndex)
        let avgNeighborPos = friendlyNeighborPhotonData |> avgPosition
        if avgNeighborPos = Position2.Zero
        then Velocity2.Zero
        else
            let diff = this.Position - avgNeighborPos
            // Don't use Direction2 for reasons of performance
            let acceleration = new Acceleration2(diff.NumericValue.Normalized())
            maxSpeed * (acceleration * elapsedTime)

    let rec Update (tracer : Tracer) (this : PhotonData)
            (gameState : GameState) (updateArgs : UpdateEventArgs) = 

        let elapsedS = updateArgs.ElapsedTimeInS
        let elapsedT = TimeSpan(elapsedS)

        // Check aliveness
        let mutable alive = true
        let collidingNeighbors = gameState.TileMap.GetNeighbors this.Position collisionRadius
        for state in filterPhotons collidingNeighbors do
            if state.PlayerIndex <> this.PlayerIndex then alive <- false

        // Compute new velocity and speed
        let goalV = velocityToGoal this elapsedT 0.3f
        let randomV = randomVelocity elapsedT 0.0f
        // FIXME interaction doesn't seem to work
        let interactionV = interactionVelocity this gameState elapsedT 0.1f
        let velocity = this.Velocity + goalV + randomV + interactionV |> capVelocity 0.4f
        let position = this.Position + velocity * elapsedT

        {
            Position = position;
            Velocity = velocity;
            Alive = alive;
            PlayerIndex = this.PlayerIndex;
        }

    let public CreatePhoton (data: PhotonData) =
        Photon (UpdatableState<PhotonData, GameState>(data, Update))

