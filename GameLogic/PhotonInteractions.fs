namespace GameLogic

open Microsoft.FSharp.Core
open Microsoft.FSharp.Collections
open Utils
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

    let private accDifference total (pos:PhotonData) = total + (pos.Position - Position2.Zero)
    let private avgPosition (photons: list<PhotonData>) =
        let sum = List.fold accDifference Difference2.Zero photons
        let count = List.length photons
        sum / (single count) + Position2.Zero

    let private accDirection total (pos:PhotonData) = total + (pos.Velocity.Direction - Direction2.Zero)
    let private avgDirection (photons: list<PhotonData>) =
        let sum = List.fold accDirection Angle.Zero photons
        let count = List.length photons
        Direction2.Zero + sum / (single count)

    let private attract (state:PhotonData) (elapsedTime:TimeSpan) (towards:list<PhotonData>) (acceleration:Acceleration) =
        if List.isEmpty towards
        then Velocity2.Zero
        else
            let attractionPoint = towards |> avgPosition
            let attractionAcceleration = Acceleration2.Towards(state.Position, attractionPoint, acceleration)
            attractionAcceleration * elapsedTime

    let private repulse (state:PhotonData) (elapsedTime:TimeSpan) (from:list<PhotonData>) (acceleration:Acceleration) =
        if List.isEmpty from
        then Velocity2.Zero
        else
            let repulsionPoint = from |> avgPosition
            let repulsionAcceleration = Acceleration2.Repulse(state.Position, repulsionPoint, acceleration)
            repulsionAcceleration * elapsedTime

    let private align (state:PhotonData) (elapsedTime:TimeSpan) (from:list<PhotonData>) (acceleration:Acceleration) =
        if List.isEmpty from
        then Velocity2.Zero
        else
            let alignDirection = from |> avgDirection
            let alignAcceleration = Acceleration2.In(alignDirection, acceleration)
            alignAcceleration * elapsedTime


    let hexagonBehaviour (state:PhotonData) (elapsedTime:TimeSpan) (gameState:GameState) =
        let isPulled (state:PhotonData) (towards:PhotonData) =
            let diff = towards.Position - state.Position
            let degrees = diff.Direction.Degrees
            let distance = diff.Length
            (
            0.0f < degrees && degrees < 90.0f ||
            180.0f < degrees && degrees < 270.0f
            ) &&
            0.02f < distance.NumericValue &&
            distance.NumericValue < 0.07f
        let maxNrInteractions = gameState.GameParameters.MaxPhotonInteractionsPerFrame
        let acceleration = accelerationFriendlyInteraction
        let neighbors = getNeighbors state gameState friendlyInteractionRadius
        let cappedNeighbors = neighbors |> takeAtMost maxNrInteractions |> Seq.toList
        let pulledTo, pushedFrom = List.partition (isPulled state) cappedNeighbors
        let vAttract = attract state elapsedTime pulledTo acceleration
        let vRepulse = repulse state elapsedTime pushedFrom acceleration
        vAttract + vRepulse


    let private gridIsPushed (state:PhotonData) (towards:PhotonData) =
        (state.Position - towards.Position).Length.NumericValue < 0.05f
    let private gridIsPulled (state:PhotonData) (towards:PhotonData) =
        not (System.Math.Abs((state.Position.X - towards.Position.X).NumericValue) < 0.03f ||
            (System.Math.Abs((state.Position.Y - towards.Position.Y).NumericValue) < 0.03f))
    let gridBehaviour (state:PhotonData) (elapsedTime:TimeSpan) (gameState:GameState) =
        let maxNrInteractions = gameState.GameParameters.MaxPhotonInteractionsPerFrame
        let acceleration = accelerationFriendlyInteraction
        let neighbors = getNeighbors state gameState friendlyInteractionRadius
        let cappedNeighbors = neighbors |> takeAtMost maxNrInteractions |> Seq.toList
        let pushedFrom = List.filter (gridIsPushed state) cappedNeighbors
        let pulledTo = List.filter (gridIsPulled state) cappedNeighbors
        let vRepulse = repulse state elapsedTime pushedFrom acceleration
        let vAttract = attract state elapsedTime pulledTo acceleration
        vAttract + vRepulse

    let swarmPushRadius = Unit 0.03f
    let swarmPullRadius = Unit 0.2f
    /// Attract but avoid collision
    let swarmBehaviour (state:PhotonData) (elapsedTime:TimeSpan) (gameState:GameState) =
        let maxNrInteractions = gameState.GameParameters.MaxPhotonInteractionsPerFrame
        let acceleration = accelerationFriendlyInteraction
        let pushingNeighbor = getSortedNeighbors state gameState swarmPushRadius |> takeAtMost 3
        if not (List.isEmpty pushingNeighbor)
        then
            repulse state elapsedTime pushingNeighbor (acceleration*5.0f)
        else
            let pullingNeighbors = getNeighbors state gameState swarmPullRadius |> takeAtMost maxNrInteractions |> Seq.toList
            attract state elapsedTime pullingNeighbors acceleration

    let smoothSwarmPushRadius = Unit 0.03f
    let smoothSwarmAlignRadius = Unit 0.1f
    let smoothSwarmPullRadius = Unit 0.2f
    /// Attract and align but avoid collision
    let smoothSwarmBehaviour (state:PhotonData) (elapsedTime:TimeSpan) (gameState:GameState) =
        let maxNrInteractions = gameState.GameParameters.MaxPhotonInteractionsPerFrame
        let acceleration = accelerationFriendlyInteraction
        let pushingNeighbors = getSortedNeighbors state gameState smoothSwarmPushRadius |> takeAtMost 3
        if not (List.isEmpty pushingNeighbors)
        then
            // Repulse
            repulse state elapsedTime pushingNeighbors (acceleration*5.0f)
        else
            let aligningNeighbors = getSortedNeighbors state gameState smoothSwarmAlignRadius |> takeAtMost 6
            if not (List.isEmpty aligningNeighbors)
            then
                // Align
                align state elapsedTime aligningNeighbors acceleration
            else
                let pullingNeighbors = getNeighbors state gameState smoothSwarmPullRadius |> takeAtMost maxNrInteractions |> Seq.toList
                // Attract
                attract state elapsedTime pullingNeighbors acceleration

    let interact (state:PhotonData) (elapsedTime:TimeSpan) (gameState:GameState) =
        let behavior =
            match state.Behavior with
            | Hexagon -> hexagonBehaviour
            | Grid -> gridBehaviour
            | Swarm -> swarmBehaviour
            | SmoothSwarm -> smoothSwarmBehaviour
        behavior state elapsedTime gameState
