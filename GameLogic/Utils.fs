module Utils
open OpenTK
open Bearded.Utilities
open System.Collections.Generic
open System

type OpenTK.Vector2 with
    member this.FromPolar(angle, radius) = Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius

let private rnd = System.Random()
let public rndSingle () = (single)(rnd.NextDouble())
let public rndInt maxValue = rnd.Next(maxValue)

type Tracer(logger: Action<string>, setNrGameObjects: Action<int>) =
    member public this.Log msg = logger.Invoke(msg)
    member public this.CountGameObjects nr = setNrGameObjects.Invoke(nr)
