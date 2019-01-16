#if INTERACTIVE
#I @"..\Bearded\libs"
#r @"amulware.Graphics.dll"
#r @"Bearded.Utilities.dll"
#endif

namespace GameLogic

module GameStateFactory =
    open Bearded.Utilities.SpaceTime
    open System
    open Bearded.Utilities.Math
    open OpenTK
    open Utils

    let rndPerturbation amplitude = (rndSingle () * 2.0f * amplitude - amplitude);

    // Create a photon on a randomized point on a circle
    let buildRandomPhoton () =
        let radius = 0.5f + rndPerturbation 0.1f
        let angle = rndSingle () * Mathf.Tau
        let angle2 = angle + Mathf.PiOver2
        let pos = Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius |> Position2
        let speed = Vector2(Mathf.Cos(angle2), Mathf.Sin(angle2)) |> Velocity2
        Photon(pos, speed * 1.0f * (1.0f/radius), 0)

    let BuildInitialGameState() =
        let photons = [0..10000] |> List.map (fun _ -> buildRandomPhoton ())
        GameState(photons)

