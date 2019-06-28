﻿#if INTERACTIVE
#r @"amulware.Graphics.dll"
#r @"Bearded.Utilities.dll"
#endif

namespace GameLogic

module GameStateFactory =
    open Bearded.Utilities.SpaceTime
    open System
    open Bearded.Utilities
    open OpenTK
    open Utils
    open System.Collections.Generic

    let rndPerturbation amplitude = (rndSingle () * 2.0f * amplitude - amplitude);

    // Create a photon on a randomized point on a circle
    let buildRandomPhoton _ : Photon.T =
        let radius = 0.5f + rndPerturbation 0.1f
        let angle = rndSingle () * Mathf.Tau
        let angle2 = angle + Mathf.PiOver2
        let pos = Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius |> Position2
        let speed = Vector2(Mathf.Cos(angle2), Mathf.Sin(angle2)) |> Velocity2
        {Position = pos; Speed = speed * 1.0f * (1.0f/radius); PoaIndex = rndInt Photon.PointsOfAttraction.Length}

    let BuildInitialGameState() =
        let photons = [0..10000] |> List.map (fun i -> Photon (new UpdatableState<Photon.T>(buildRandomPhoton i, Photon.Update)))
        GameState(new List<GameObject>(photons))

