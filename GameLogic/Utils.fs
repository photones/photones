namespace GameLogic
open OpenTK
open Bearded.Utilities
open System.Collections.Generic
open System

type Tracer(logger: Action<string>, setNrGameObjects: Action<int>) =
    member public this.Log msg = if not (Object.Equals(logger, null)) then logger.Invoke(msg) else ()
    member public this.CountGameObjects nr = if not (Object.Equals(setNrGameObjects, null)) then setNrGameObjects.Invoke(nr) else ()

module Utils =

    type OpenTK.Vector2 with
        member this.FromPolar(angle, radius) = Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius

    let private rnd = System.Random()
    let public rndSingle () = (single)(rnd.NextDouble())
    let public rndInt maxValue = rnd.Next(maxValue)

