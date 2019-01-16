module Utils
open OpenTK
open Bearded.Utilities.Math

type OpenTK.Vector2 with
    member this.FromPolar(angle, radius) = Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius

let public rnd = System.Random()
let public rndSingle () = (single)(rnd.NextDouble())
