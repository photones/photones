#if INTERACTIVE
#r @"amulware.Graphics.dll"
#r @"Bearded.Utilities.dll"
#r @"OpenTK.dll"
#endif

namespace GameLogic

open Bearded.Utilities.SpaceTime
open Utils


module public Photon =
    [<Struct>]
    type public T =
        {Position: Position2; Speed: Velocity2; PoaIndex: int}


    let private getNumber () = rndSingle () - 0.5f
    let private smallRandomVelocity () =
        Velocity2(getNumber (), getNumber ()) * 0.1f

    let private capVelocity maxSpeed (v: Velocity2) =
        if v.Length.NumericValue > maxSpeed then new Velocity2(v.NumericValue.Normalized() * maxSpeed) else v

    let private capAccToGoal = capVelocity 0.2f
    let private capTotal = capVelocity 0.8f
        
    let private attractionRadius = 0.2f

    let PointsOfAttraction = [
        Position2(-0.1f,0.3f);
        Position2(0.6f,0.1f);
        Position2(0.0f,-0.6f);
        Position2(-0.3f,0.6f);
        ]

    let private PointOfAttraction (this : T) : Position2 = PointsOfAttraction.[this.PoaIndex]
    let private HasReachedPointOfAttraction (this : T) = Unit.op_LessThan((this.Position - PointOfAttraction this).Length, Unit(attractionRadius))

    let private VelocityToGoal (this : T) (elapsedTime: TimeSpan) =
        let diff = PointOfAttraction this - this.Position
        let acceleration = if diff.Length = Unit.Zero then new Acceleration2(0.0f, 0.0f) else new Acceleration2(diff.NumericValue.Normalized() * 3.0f)
        acceleration * elapsedTime

    let Update (this : T) (elapsed : TimeSpan) = 
        let vToGoal = VelocityToGoal this elapsed
        let velocity =
            this.Speed
            + capAccToGoal vToGoal
            + (smallRandomVelocity ())
            |> capTotal
        let position = this.Position + velocity * elapsed
        let pointOfAttractionIndex = if HasReachedPointOfAttraction this then (this.PoaIndex + 1) % PointsOfAttraction.Length else this.PoaIndex
        {Position = position; Speed = velocity; PoaIndex = pointOfAttractionIndex}

