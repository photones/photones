namespace GameLogic

open Microsoft.FSharp.Core
open Microsoft.FSharp.Collections
open Utils
open Bearded.Utilities.SpaceTime
open LibraryExtensions

module public Photon =

    type T = UpdatableState<PhotonData, GameState>

    let private accelerationGoal = Acceleration 0.3f
    let private accelerationRandom = Acceleration 0.0001f
    let private accelerationFriendlyInteraction = Acceleration 0.15f
    let private accelerationHostileInteraction = Acceleration 0.15f
    let private maxSpeed = Speed 0.4f
    let private hostileInteractionRadius = Unit 0.03f
    let private friendlyInteractionRadius = Unit 0.03f

    let private capVelocity (v:Velocity2) =
        if v.Length.NumericValue > maxSpeed.NumericValue
        then Velocity2(v.NumericValue.Normalized() * maxSpeed.NumericValue)
        else v

    let private dvGoal (state:PhotonData) (gameState:GameState) (elapsedTime:TimeSpan) =
        let player = Player.getPlayerById gameState state.PlayerId
        let attractionPoint = player.State.Target
        let acceleration = Acceleration2.Towards(state.Position, attractionPoint, accelerationGoal)
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

    let private isFriendly (state:PhotonData) (other:PhotonData) =
        other.PlayerId = state.PlayerId

    let private isHostile (state:PhotonData) (other:PhotonData) =
        other.PlayerId <> state.PlayerId

    let private repulse (state:PhotonData) (elapsedTime:TimeSpan)
            (acceleration:Acceleration) (from:seq<PhotonData>) maxNrInteractions =
        if Seq.isEmpty from
        then Velocity2.Zero
        else
            let repulsionPoint = from |> takeAtMost maxNrInteractions |> avgPosition
            let acceleration = Acceleration2.Towards(repulsionPoint, state.Position, acceleration)
            acceleration * elapsedTime

    let private attract (state:PhotonData) (elapsedTime:TimeSpan)
            (acceleration:Acceleration) (towards:seq<PhotonData>) maxNrInteractions =
        if Seq.isEmpty towards
        then Velocity2.Zero
        else
            let attractionPoint = towards |> takeAtMost maxNrInteractions |> avgPosition
            let acceleration = Acceleration2.Towards(state.Position, attractionPoint, acceleration)
            acceleration * elapsedTime

    let private dvHostileInteraction (this:T) (gameState:GameState) (elapsedTime:TimeSpan) =
        let maxNrInteractions = gameState.GameParameters.MaxPhotonInteractionsPerFrame
        let acceleration = accelerationHostileInteraction 
        let state = this.State
        let neighbors = getNeighbors this gameState hostileInteractionRadius
        let hostiles = neighbors |> Seq.filter (isHostile state)
        match state.Behavior with
        | Shy -> repulse state elapsedTime acceleration hostiles maxNrInteractions
        | Neutral -> Velocity2.Zero
        | Aggressive -> attract state elapsedTime acceleration hostiles maxNrInteractions

    let private dvFriendlyInteraction (this:T) (gameState:GameState) (elapsedTime:TimeSpan) =
        let maxNrInteractions = gameState.GameParameters.MaxPhotonInteractionsPerFrame
        let state = this.State
        let neighbors = getNeighbors this gameState friendlyInteractionRadius
        let friendlies = neighbors |> Seq.filter (isFriendly state)
        repulse state elapsedTime accelerationFriendlyInteraction friendlies maxNrInteractions

    let isColliding (this:T) (gameState:GameState) = 
        let collidingPhotons = getNeighbors this gameState this.State.Size
        let collidingHostiles = collidingPhotons |> Seq.filter (isHostile this.State)
        Seq.isEmpty collidingHostiles |> not

    let rec update (this:T) (gameState:GameState) (elapsedS:TimeSpan) = 
        let state = this.State

        // Check aliveness
        let isOnTileMap = gameState.TileMap.IsOnTileMap state.Position
        let mutable alive = isOnTileMap && (isColliding this gameState |> not) 

        // Compute new velocity and position
        let ΔV1 = dvGoal state gameState elapsedS
        let ΔV2 = dvRandom elapsedS
        let ΔV3 = dvFriendlyInteraction this gameState elapsedS
        let ΔV4 = dvHostileInteraction this gameState elapsedS

        let ΔV = ΔV1 + ΔV2 + ΔV3 + ΔV4
        let velocity = state.Velocity + ΔV |> capVelocity
        let position = state.Position + velocity * elapsedS

        {
            Position = position;
            Velocity = velocity;
            Size = state.Size;
            Alive = alive;
            PlayerId = state.PlayerId;
            Behavior = state.Behavior;
        }

    let createPhoton (data: PhotonData) =
        Photon (T(data, update))

