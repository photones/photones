#if INTERACTIVE
#r @"amulware.Graphics.dll"
#r @"Bearded.Utilities.dll"
#r @"OpenTK.dll"
#endif

namespace GameLogic

open Bearded.Utilities.SpaceTime
open Utils

[<Struct>]
type public PhotonAttributes = {Position: Position2; Speed: Velocity2; PoaIndex: int}

type Photon(initialAttr: PhotonAttributes) =

    let getNumber () = rndSingle () - 0.5f
    let smallRandomVelocity () =
        Velocity2(getNumber (), getNumber ()) * 0.1f

    let capVelocity maxSpeed (v: Velocity2) =
        if v.Length.NumericValue > maxSpeed then new Velocity2(v.NumericValue.Normalized() * maxSpeed) else v

    let capAccToGoal = capVelocity 0.2f
    let capTotal = capVelocity 0.8f
        
    let attractionRadius = 0.2f

    let mutable futureAttributes : PhotonAttributes = initialAttr
    let mutable attributes : PhotonAttributes = initialAttr

    member public this.Position = attributes.Position
    member public this.PoaIndex = attributes.PoaIndex
    member public this.Speed = attributes.Speed

    static member PointsOfAttraction = [
        Position2(-0.1f,0.3f);
        Position2(0.6f,0.1f);
        Position2(0.0f,-0.6f);
        Position2(-0.3f,0.6f);
        ]

    member private this.PointOfAttraction : Position2 = Photon.PointsOfAttraction.[this.PoaIndex]
    member private this.HasReachedPointOfAttraction = Unit.op_LessThan((this.Position - this.PointOfAttraction).Length, Unit(attractionRadius))

    member private this.VelocityToGoal (elapsedTime: TimeSpan) =
        let diff = this.PointOfAttraction - this.Position
        let acceleration = if diff.Length = Unit.Zero then new Acceleration2(0.0f, 0.0f) else new Acceleration2(diff.NumericValue.Normalized() * 3.0f)
        acceleration * elapsedTime
        

    member public this.Update (elapsedTime: TimeSpan) =
        let vToGoal = this.VelocityToGoal elapsedTime
        let velocity =
            this.Speed
            + capAccToGoal vToGoal
            + (smallRandomVelocity ())
            |> capTotal
        let position = this.Position + velocity * elapsedTime
        let pointOfAttractionIndex = if this.HasReachedPointOfAttraction then (this.PoaIndex + 1) % Photon.PointsOfAttraction.Length else this.PoaIndex
        futureAttributes <- {Position = position; Speed = velocity; PoaIndex = pointOfAttractionIndex}

    member public this.Refresh () =
        attributes <- futureAttributes

