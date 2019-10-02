namespace GameLogic
open OpenTK
open Bearded.Utilities

module OpenTkUtils =

    type OpenTK.Vector2 with
        member this.FromPolar(angle, radius) = Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius
