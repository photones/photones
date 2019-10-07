namespace GameLogic

open Bearded.Utilities.SpaceTime
open Utils
open Microsoft.FSharp.Core
open Microsoft.FSharp.Collections

module public Photon =

    let private accelerationToGoal = 0.3f
    let private accelerationRandom = 0.01f
    let private accelerationInteraction = 0.2f
    let private maxSpeed = 0.4f
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

    let private velocityToGoal
            (state:PhotonData) (elapsedTime:TimeSpan) (accelerationConstant:single) =
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
        accelerationConstant * acceleration * elapsedTime

    let private randomSingle2 () = (randomSingle () - 0.5f) * 2.0f
    let private randomVelocity (elapsedTime:TimeSpan) (accelerationConstant:single) =
        let acceleration = new Acceleration2(randomSingle2 (), randomSingle2 ())
        accelerationConstant * acceleration * elapsedTime

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

    let private repulse (this:UpdatableState<PhotonData, GameState>)
            (elapsedTime:TimeSpan) (accelerationConstant:single) (from:seq<PhotonData>) =
        if Seq.isEmpty from
        then Velocity2.Zero
        else
            let repulsionPosition =
                from |> takeAtMost maxNrInteractions |> avgPhotonPosition
            let diff = this.State.Position - repulsionPosition
            // Don't use Direction2 for reasons of performance
            let acceleration =
                if diff.Length = Unit.Zero
                then Acceleration2.Zero
                else new Acceleration2(diff.NumericValue.Normalized())
            accelerationConstant * acceleration * elapsedTime

    /// Move away from friendly neighbors that are within interaction radius
    let private interactionVelocity (this:UpdatableState<PhotonData, GameState>)
            (gameState:GameState) (elapsedTime:TimeSpan) (accelerationConstant:single) = 
        let state = this.State
        let neighbors = gameState.TileMap.GetObjects this.State.Position interactionRadius
        let repulseFrom =
            match state.Behavior with
            | Shy -> filterPhotons this neighbors
            | Neutral ->
                filterPhotons this neighbors |>
                Seq.filter (fun neighbor -> neighbor.PlayerIndex = this.State.PlayerIndex)
            | Aggressive -> Seq.empty
        repulse this elapsedTime accelerationConstant repulseFrom

    let rec Update (tracer : Tracer) (this : UpdatableState<PhotonData, GameState>)
            (gameState : GameState) (elapsedS : TimeSpan) = 

        let state = this.State

        // Check aliveness
        let mutable alive = true
        let collidingNeighbors = gameState.TileMap.GetObjects state.Position collisionRadius
        for collidingState in filterPhotons this collidingNeighbors do
            if collidingState.PlayerIndex <> state.PlayerIndex then alive <- false

        // Compute new velocity and speed
        let goalDV = velocityToGoal state elapsedS accelerationToGoal
        let randomDV = randomVelocity elapsedS accelerationRandom
        let interactionDV = interactionVelocity this gameState elapsedS accelerationInteraction
        let velocity = state.Velocity + goalDV + randomDV + interactionDV |> capVelocity maxSpeed
        let position = state.Position + velocity * elapsedS

        {
            Position = position;
            Velocity = velocity;
            Alive = alive;
            PlayerIndex = state.PlayerIndex;
            Behavior = state.Behavior;
        }

    let CreatePhoton (data: PhotonData) =
        Photon (UpdatableState<PhotonData, GameState>(data, Update))

