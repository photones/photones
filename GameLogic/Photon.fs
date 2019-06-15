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

    let getNumber () = rndSingle () - 0.5f
    let smallRandomVelocity () =
        Velocity2(getNumber (), getNumber ()) * 0.1f
        
    let attractionRadius = 0.2f
    let pointsOfAttraction = [
        Position2(-0.1f,0.3f);
        Position2(0.6f,0.1f);
        Position2(0.0f,-0.6f);
        Position2(-0.3f,0.6f);
        ]


    let private pointOfAttraction (this: T): Position2 = pointsOfAttraction.[this.PoaIndex]
    let private hasReachedPointOfAttraction (this:T) =  Unit.op_LessThan((this.Position - pointOfAttraction this).Length, Unit(attractionRadius))

    let private velocityToGoal (elapsedTime: TimeSpan) (this: T) =
        let diff = pointOfAttraction this - this.Position
        //let acceleration = Acceleration2.In(diff.Direction, new Acceleration(3.0f))
        let acceleration = new Acceleration2(diff.NumericValue.Normalized() * 3.0f)
        acceleration * elapsedTime
        
    let capVelocity maxSpeed (v: Velocity2) =
        //if v.Length.NumericValue > maxSpeed then Velocity2.In(v.Direction, Speed(maxSpeed)) else v
        if v.Length.NumericValue > maxSpeed then new Velocity2(v.NumericValue.Normalized() * maxSpeed) else v

    let capAccToGoal = capVelocity 0.2f
    let capTotal = capVelocity 0.8f

    let update (elapsedTime: TimeSpan) (this: T) : T =

        let vToGoal = velocityToGoal elapsedTime this

        let velocity =
            this.Speed
            + capAccToGoal vToGoal
            + (smallRandomVelocity ())
            |> capTotal
        let position = this.Position + velocity * elapsedTime
        let pointOfAttractionIndex = if hasReachedPointOfAttraction this then (this.PoaIndex + 1) % pointsOfAttraction.Length else this.PoaIndex
        {Position = position; Speed = velocity; PoaIndex = pointOfAttractionIndex}

