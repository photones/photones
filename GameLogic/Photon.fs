namespace GameLogic

open Bearded.Utilities.SpaceTime
open Utils
open amulware.Graphics
open Microsoft.FSharp.Core
open Microsoft.FSharp.Collections

module public Photon =

    let private interactionRadius = Unit(0.01f)
    let private collisionRadius = Unit(0.003f)
    /// The max number of interactions that will be computed.
    /// The higher the number, the better and smoother the interactions will be, but it comes at a
    /// cost of performance. If the interaction is programmed to move away from neighbors, and the
    /// neighbors are all on a horizontal line, you would want to move away from that line,
    /// orthogonally. However, what happens if this number is low and the interactionRadius is high,
    /// is that the average neighbor position will deviate horizontally significantly more than
    /// vertically. This means that we will move horizontally away instead of vertically.
    let private maxNrInteractions = 5

    let private capVelocity maxSpeed (v:Velocity2) =
        if v.Length.NumericValue > maxSpeed
        then Velocity2(v.NumericValue.Normalized() * maxSpeed)
        else v

    let private velocityToGoal (state:PhotonData) (elapsedTime:TimeSpan) (maxSpeed:single) =
        // Hardcoded behaviour based on player index
        let pointOfAttraction =
            if state.PlayerIndex = 0uy
            then Position2(0.9f, 0.0f)
            else Position2(-0.9f, 0.0f)
        let diff = pointOfAttraction - state.Position
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

    let private accDifference total (pos:PhotonData) = total + (pos.Position - Position2.Zero)
    let private avgPhotonPosition (photons: list<PhotonData>) =
        let sum = List.fold accDifference Difference2.Zero photons
        let count = List.length photons
        sum / (single count) + Position2.Zero

    let private filterPhotons (this:UpdatableState<PhotonData, GameState>) objects = seq{
        for n in objects do
            match n with
            | Planet _ -> ()
            | Photon d -> if this <> d then yield d.State
        }

    /// Move away from friendly neighbors that are within interaction radius
    let private interactionVelocity (this:UpdatableState<PhotonData, GameState>)
            (gameState:GameState) (elapsedTime:TimeSpan) (maxSpeed:single) = 
        let neighbors = gameState.TileMap.GetObjects this.State.Position interactionRadius
        let friendlyNeighborPhotonData =
            filterPhotons this neighbors |>
            Seq.filter (fun data -> data.PlayerIndex = this.State.PlayerIndex)
        if Seq.isEmpty friendlyNeighborPhotonData
        then Velocity2.Zero
        else
            let repulsionPosition =
                friendlyNeighborPhotonData |> takeAtMost maxNrInteractions |> avgPhotonPosition
            let diff = this.State.Position - repulsionPosition
            // Don't use Direction2 for reasons of performance
            let acceleration =
                if diff.Length = Unit.Zero
                then Acceleration2.Zero
                else new Acceleration2(diff.NumericValue.Normalized())
            maxSpeed * (acceleration * elapsedTime)

    let rec Update (tracer : Tracer) (this : UpdatableState<PhotonData, GameState>)
            (gameState : GameState) (updateArgs : UpdateEventArgs) = 

        let elapsedS = updateArgs.ElapsedTimeInS
        let elapsedT = TimeSpan(elapsedS)
        let state = this.State

        // Check aliveness
        let mutable alive = true
        let collidingNeighbors = gameState.TileMap.GetObjects state.Position collisionRadius
        for collidingState in filterPhotons this collidingNeighbors do
            if collidingState.PlayerIndex <> state.PlayerIndex then alive <- false

        // Compute new velocity and speed
        let goalDV = velocityToGoal state elapsedT 0.3f
        let randomDV = randomVelocity elapsedT 0.01f
        let interactionDV = interactionVelocity this gameState elapsedT 0.1f
        let velocity = state.Velocity + goalDV + randomDV + interactionDV |> capVelocity 0.4f
        let position = state.Position + velocity * elapsedT

        {
            Position = position;
            Velocity = velocity;
            Alive = alive;
            PlayerIndex = state.PlayerIndex;
        }

    let CreatePhoton (data: PhotonData) =
        Photon (UpdatableState<PhotonData, GameState>(data, Update))

