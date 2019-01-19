#if INTERACTIVE
#r @"amulware.Graphics.dll"
#r @"Bearded.Utilities.dll"
#r @"OpenTK.dll"
#endif

namespace GameLogic

open Bearded.Utilities.SpaceTime
open Utils

module Photon =
    [<Struct>]
    type public T = {Position: Position2; Speed: Velocity2; PoaIndex: int}

    let smallRandomVelocity () =
        let getNumber () = rndSingle () - 0.5f
        Velocity2(getNumber (), getNumber ()) * 0.1f
        
    let attractionRadius = 0.2f
    let pointsOfAttraction = [
        Position2(-0.1f,0.3f);
        Position2(0.6f,0.1f);
        Position2(0.0f,-0.6f);
        Position2(-0.3f,0.6f);
        ]


    let private pointOfAttraction this: Position2 = pointsOfAttraction.[this.PoaIndex]
    let private hasReachedPointOfAttraction this =  Unit.op_LessThan((this.Position - pointOfAttraction this).Length, Unit(attractionRadius))

    let private velocityToGoal (elapsedTime: TimeSpan) (this: T) =
        let diff = pointOfAttraction this - this.Position
        let acceleration = Acceleration2.In(diff.Direction, Acceleration(3.0f))
        acceleration * elapsedTime

    let private velocityNeighborReaction (elapsedTime: TimeSpan) (this: T) (neighbors: T list) =
        let diffs = List.map (fun n -> n.Position - this.Position) neighbors
        let accelerations = List.map (fun (d: Difference2) -> Acceleration2.In(-d.Direction, Acceleration(1.0f/d.Length.NumericValue))) diffs
        let avgAcceleration = List.sum accelerations
        avgAcceleration * elapsedTime
        
    let capVelocity maxSpeed (v: Velocity2) =
        if v.Length.NumericValue > maxSpeed then Velocity2.In(v.Direction, Speed(maxSpeed)) else v

    let update (elapsedTime: TimeSpan) (this: T, neighbors: T list) : T =
        let capAccToGoal = capVelocity 0.2f
        let capAccNeighborReaction = capVelocity 0.1f
        let capTotal = capVelocity 0.8f

        let vToGoal = velocityToGoal elapsedTime this
        let vNeighborReaction = velocityNeighborReaction elapsedTime this neighbors

        let velocity =
            this.Speed
            + capAccToGoal vToGoal
            + capAccNeighborReaction vNeighborReaction
            |> capTotal
        let position = this.Position + velocity * elapsedTime
        let pointOfAttractionIndex = if hasReachedPointOfAttraction this then (this.PoaIndex + 1) % pointsOfAttraction.Length else this.PoaIndex
        {Position = position; Speed = velocity; PoaIndex = pointOfAttractionIndex}

