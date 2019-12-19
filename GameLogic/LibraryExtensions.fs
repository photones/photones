namespace GameLogic
open OpenTK
open Bearded.Utilities
open Bearded.Utilities.SpaceTime

module LibraryExtensions =

    type OpenTK.Vector2 with
        member this.FromPolar(angle, radius) = Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius

    type Mass with
        static member CenterOfMass (massFunction: Position2 -> Mass) (points: list<Position2>) =
            let totalMass = List.sumBy massFunction points
            let w point = (point - Position2.Zero) * (massFunction point / totalMass)
            let relativeCenterOfMass = List.sumBy w points
            Position2.Zero + relativeCenterOfMass

    type Acceleration2 with
        static member Towards(from:Position2, towards:Position2, by:Acceleration) = 
            let diff = towards - from
            // Don't use Direction2 for reasons of performance
            if diff.Length = Unit.Zero
            then Acceleration2.Zero
            else new Acceleration2(diff.NumericValue.Normalized()) * by.NumericValue

        static member Repulse(current:Position2, repulsionPoint:Position2, by:Acceleration) = 
            let diff = current - repulsionPoint
            // Don't use Direction2 for reasons of performance
            if diff.Length = Unit.Zero
            then Acceleration2.Zero
            else new Acceleration2(diff.NumericValue.Normalized()) * by.NumericValue
