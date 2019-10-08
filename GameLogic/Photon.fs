namespace GameLogic

open Microsoft.FSharp.Core
open Microsoft.FSharp.Collections
open Utils
open Bearded.Utilities.SpaceTime
open LibraryExtensions

module public Photon =

    type T = UpdatableState<PhotonData, GameState>

    let private accelerationToGoal = Acceleration 0.3f
    let private accelerationRandom = Acceleration 0.01f
    let private accelerationFriendlyInteraction = Acceleration 0.1f
    let private accelerationHostileInteraction = Acceleration 0.1f
    let private maxSpeed = Speed 0.4f
    let private hostileInteractionRadius = Unit 0.01f
    let private friendlyInteractionRadius = Unit 0.01f
    let private collisionRadius = Unit 0.003f
    /// The max number of interactions that will be computed.
    /// The higher the number, the better and smoother the interactions will be, but it comes at a
    /// cost of performance. If the interaction is programmed to move away from neighbors, and the
    /// neighbors are all on a horizontal line, you would want to move away from that line,
    /// orthogonally. However, what happens if this number is low and the interactionRadius is high,
    /// is that the average neighbor position will deviate horizontally significantly more than
    /// vertically. This means that we will move horizontally away instead of vertically.
    let private maxNrInteractions = 5

    let private capVelocity (v:Velocity2) =
        if v.Length.NumericValue > maxSpeed.NumericValue
        then Velocity2(v.NumericValue.Normalized() * maxSpeed.NumericValue)
        else v

    let private isFriendly (state:PhotonData) (other:PhotonData) =
        other.PlayerIndex = state.PlayerIndex

    let private isHostile (state:PhotonData) (other:PhotonData) =
        other.PlayerIndex <> state.PlayerIndex

    let private dvGoal (state:PhotonData) (elapsedTime:TimeSpan) =
        // Hardcoded behaviour based on player index
        let attractionPoint =
            if state.PlayerIndex = 0uy
            then Position2(0.9f, 0.0f)
            else Position2(-0.9f, 0.0f)
        let acceleration = Acceleration2.Towards(state.Position, attractionPoint, accelerationToGoal)
        acceleration * elapsedTime

    let private randomSingle2 () = (randomSingle () - 0.5f) * 2.0f
    let private dvRandom (elapsedTime:TimeSpan) =
        let randomPosition = Position2(randomSingle2 (), randomSingle2 ())
        let acceleration = Acceleration2.Towards(Position2.Zero, randomPosition, accelerationRandom)
        acceleration * elapsedTime

    let private accDifference total (pos:PhotonData) = total + (pos.Position - Position2.Zero)
    let private avgPosition (photons: list<PhotonData>) =
        let sum = List.fold accDifference Difference2.Zero photons
        let count = List.length photons
        sum / (single count) + Position2.Zero

    let private getNeighbors (this:T) (gameState:GameState) (radius:Unit) =
        let neighbors = gameState.TileMap.GetObjects this.State.Position radius
        seq {
            for n in neighbors do
                match n with
                | Planet _ -> ()
                | Photon d -> if this <> d then yield d.State
        }

    let private repulse (state:PhotonData) (elapsedTime:TimeSpan)
            (acceleration:Acceleration) (from:seq<PhotonData>) =
        if Seq.isEmpty from
        then Velocity2.Zero
        else
            let repulsionPoint = from |> takeAtMost maxNrInteractions |> avgPosition
            let acceleration = Acceleration2.Towards(repulsionPoint, state.Position, acceleration)
            acceleration * elapsedTime

    let private attract (state:PhotonData) (elapsedTime:TimeSpan)
            (acceleration:Acceleration) (towards:seq<PhotonData>) =
        if Seq.isEmpty towards
        then Velocity2.Zero
        else
            let attractionPoint = towards |> takeAtMost maxNrInteractions |> avgPosition
            let acceleration = Acceleration2.Towards(state.Position, attractionPoint, acceleration)
            acceleration * elapsedTime

    let private dvHostileInteraction (this:T) (gameState:GameState)
            (elapsedTime:TimeSpan) =
        let state = this.State
        let neighbors = getNeighbors this gameState hostileInteractionRadius
        let hostiles = neighbors |> Seq.filter (isHostile state)
        match state.Behavior with
        | Shy -> repulse state elapsedTime accelerationHostileInteraction hostiles
        | Neutral -> Velocity2.Zero
        | Aggressive -> attract state elapsedTime accelerationHostileInteraction hostiles

    let private dvFriendlyInteraction (this:T) (gameState:GameState) (elapsedTime:TimeSpan) =
        let state = this.State
        let neighbors = getNeighbors this gameState friendlyInteractionRadius
        let friendlies = neighbors |> Seq.filter (isFriendly state)
        repulse state elapsedTime accelerationFriendlyInteraction friendlies

    let rec Update (tracer:Tracer) (this:T) (gameState:GameState) (elapsedS:TimeSpan) = 
        let state = this.State

        // Check aliveness
        let mutable alive = true
        let collidingPhotons = getNeighbors this gameState collisionRadius
        let collidingHostiles = collidingPhotons |> Seq.filter (isHostile state)
        if Seq.isEmpty collidingHostiles |> not then alive <- false

        // Compute new velocity and position
        let ΔV1 = dvGoal state elapsedS
        let ΔV2 = dvRandom elapsedS
        let ΔV3 = dvFriendlyInteraction this gameState elapsedS
        let ΔV4 = dvHostileInteraction this gameState elapsedS

        let ΔV = ΔV1 + ΔV2 + ΔV3 + ΔV4
        let velocity = state.Velocity + ΔV |> capVelocity
        let position = state.Position + velocity * elapsedS

        {
            Position = position;
            Velocity = velocity;
            Alive = alive;
            PlayerIndex = state.PlayerIndex;
            Behavior = state.Behavior;
        }

    let CreatePhoton (data: PhotonData) =
        Photon (T(data, Update))

