namespace GameLogic

open Microsoft.FSharp.Core
open Microsoft.FSharp.Collections
open Utils
open Bearded.Utilities
open Bearded.Utilities.SpaceTime
open Bearded.Utilities.Geometry
open LibraryExtensions

module public PhotonInteractions =

    let private hostileInteractionRadius = Unit 0.03f
    let private accelerationHostileInteraction = Acceleration 0.15f
    let private friendlyInteractionRadius = Unit 0.1f
    let private accelerationFriendlyInteraction = Acceleration 3.0f
   
    let private isFriendly (state:PhotonData) (other:PhotonData) =
        other.PlayerId = state.PlayerId

    let public getNeighbors (state:PhotonData) (gameState:GameState) (radius:Unit) =
        let neighbors = gameState.TileMap.GetObjects state.Position radius
        seq {
            for n in neighbors do
                match n with
                | Planet _ -> ()
                | Photon d -> if state.Position <> d.State.Position then yield d.State
        }

    let public getSortedNeighbors (state:PhotonData) (gameState:GameState) (radius:Unit) =
        let neighbors = gameState.TileMap.GetObjectsSortedByDistance state.Position radius
        seq {
            for n in neighbors do
                match n with
                | Planet _ -> ()
                | Photon d -> if state.Position <> d.State.Position then yield d.State
        }

    /// Always weigh with some continuous function to avoid twitchy behaviour.
    /// A photon near the maximum interaction radius should almost have no effect.
    let private weight (maxDistance:Unit) (origin:Position2) (point:Position2) = 
        let distance = (point - origin).Length
        let distanceFraction = distance / maxDistance
        // To make it harder for photons further away to outrule a single close photon,
        // we take a root of the distance
        let rootedDistanceFraction = Mathf.Pow(distanceFraction, 0.1f)
        // Never give back a mass of zero
        new Mass(1.00001f - rootedDistanceFraction)
    let private weighedCenterOfNeighborhood (maxDistance:Unit) (origin:PhotonData) (photons: list<PhotonData>) =
        let points = List.map (fun p -> p.Position) photons
        Mass.CenterOfMass (weight maxDistance origin.Position) points

    /// TODO: Always weigh inversely by distance to avoid funny unwanted equilibria and twitchy behaviour
    let private accDirection total (pos:PhotonData) = total + (pos.Velocity.Direction - Direction2.Zero)
    let private avgDirection (photons: list<PhotonData>) =
        let sum = List.fold accDirection Angle.Zero photons
        let count = List.length photons
        Direction2.Zero + sum / (single count)

    let private attract (maxDistance:Unit) (state:PhotonData) (elapsedTime:TimeSpan) (towards:list<PhotonData>) (acceleration:Acceleration) =
        if List.isEmpty towards
        then Velocity2.Zero
        else
            let attractionPoint = towards |> weighedCenterOfNeighborhood maxDistance state
            let attractionAcceleration = Acceleration2.Towards(state.Position, attractionPoint, acceleration)
            attractionAcceleration * elapsedTime

    let private repulse (maxDistance:Unit) (state:PhotonData) (elapsedTime:TimeSpan) (from:list<PhotonData>) (acceleration:Acceleration) =
        if List.isEmpty from
        then Velocity2.Zero
        else
            let repulsionPoint = from |> weighedCenterOfNeighborhood maxDistance state
            let repulsionAcceleration = Acceleration2.Repulse(state.Position, repulsionPoint, acceleration)
            repulsionAcceleration * elapsedTime

    let private align (state:PhotonData) (elapsedTime:TimeSpan) (from:list<PhotonData>) (acceleration:Acceleration) =
        if List.isEmpty from
        then Velocity2.Zero
        else
            let alignDirection = from |> avgDirection
            let alignAcceleration = Acceleration2.In(alignDirection, acceleration)
            alignAcceleration * elapsedTime


    let swarmPushRadius = Unit 0.03f
    let swarmAlignRadius = Unit 0.1f
    let swarmPullRadius = Unit 0.2f
    /// Attract and align but avoid collision
    let swarmBehaviour (state:PhotonData) (elapsedTime:TimeSpan) (gameState:GameState) =
        let maxNrInteractions = gameState.GameParameters.MaxPhotonInteractionsPerFrame
        let acceleration = accelerationFriendlyInteraction
        let pushingNeighbors = getSortedNeighbors state gameState swarmPushRadius |> takeAtMost 9
        if not (List.isEmpty pushingNeighbors)
        then
            // Repulse
            repulse swarmPushRadius state elapsedTime pushingNeighbors (acceleration*2.0f)
        else
            let aligningNeighbors = getSortedNeighbors state gameState swarmAlignRadius |> takeAtMost 6
            if not (List.isEmpty aligningNeighbors)
            then
                // Align
                align state elapsedTime aligningNeighbors acceleration
            else
                let pullingNeighbors = getNeighbors state gameState swarmPullRadius |> takeAtMost maxNrInteractions |> Seq.toList
                // Attract
                attract swarmPullRadius state elapsedTime pullingNeighbors acceleration

    let interact (state:PhotonData) (elapsedTime:TimeSpan) (gameState:GameState) =
        let behavior =
            match state.Behavior with
            | Swarm -> swarmBehaviour
        behavior state elapsedTime gameState
