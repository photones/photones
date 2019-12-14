namespace GameLogic

open Microsoft.FSharp.Core
open Microsoft.FSharp.Collections
open Utils
open Bearded.Utilities.SpaceTime
open LibraryExtensions

module public Photon =

    type T = UpdatableState<PhotonData, GameState>

    let private frictionFraction = 0.0f // Fraction of speed that is lost per second
    let private accelerationGoal = Acceleration 0.5f
    let private accelerationRandom = Acceleration 0.0001f
    let private maxSpeed = Speed 0.4f

    let private capVelocity (v:Velocity2) =
        if v.Length.NumericValue > maxSpeed.NumericValue
        then Velocity2(v.NumericValue.Normalized() * maxSpeed.NumericValue)
        else v

    let private dvGoal (state:PhotonData) (gameState:GameState) (elapsedTime:TimeSpan) =
        let player = Player.getPlayerById gameState state.PlayerId
        let attractionPoint = player.State.Target
        let distance = (state.Position - attractionPoint).Length
        if Unit.op_LessThan(distance, Unit 0.0f)
        then Velocity2.Zero
        else 
            let acceleration = Acceleration2.Towards(state.Position, attractionPoint, accelerationGoal)
            acceleration * elapsedTime

    let private randomSingle2 () = (randomSingle () - 0.5f) * 2.0f
    let private dvRandom (elapsedTime:TimeSpan) =
        let randomPosition = Position2(randomSingle2 (), randomSingle2 ())
        let acceleration = Acceleration2.Towards(Position2.Zero, randomPosition, accelerationRandom)
        acceleration * elapsedTime

    let private isHostile (state:PhotonData) (other:PhotonData) =
        other.PlayerId <> state.PlayerId

    let private dvInteraction (state:PhotonData) (gameState:GameState) (elapsedTime:TimeSpan) =
        PhotonInteractions.interact state elapsedTime gameState

    let isColliding (state:PhotonData) (gameState:GameState) = 
        let collidingPhotons = PhotonInteractions.getNeighbors state gameState state.Size
        let collidingHostiles = collidingPhotons |> Seq.filter (isHostile state)
        Seq.isEmpty collidingHostiles |> not

    let computeFrictionMultiplier (elapsedS:TimeSpan) = 
        let frictionMultiplierPerSecond = 1.0f - frictionFraction |> float
        System.Math.Pow(frictionMultiplierPerSecond, elapsedS.NumericValue) |> single

    let rec update (this:T) (gameState:GameState) (elapsedS:TimeSpan) = 
        let state = this.State

        // Check aliveness
        let isOnTileMap = gameState.TileMap.IsOnTileMap state.Position
        let mutable alive = isOnTileMap && (isColliding state gameState |> not) 

        // Compute new velocity and position
        let ΔV1 = dvGoal state gameState elapsedS
        let ΔV2 = dvRandom elapsedS
        let ΔV3 = dvInteraction state gameState elapsedS

        let frictionMultiplier = computeFrictionMultiplier elapsedS
        let ΔV = ΔV1 + ΔV2 + ΔV3
        let velocity = (state.Velocity + ΔV) * frictionMultiplier |> capVelocity
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

