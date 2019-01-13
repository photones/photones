#if INTERACTIVE
#I @"..\Bearded\libs"
#r @"amulware.Graphics.dll"
#r @"Bearded.Utilities.dll"
#endif

namespace GameLogic

module GameStateFactory =
    open Bearded.Utilities.SpaceTime
    open System

    let rnd = Random()

    let buildRandomPhoton () =
        let rndCoord () = (single)(rnd.NextDouble()) - 0.5f;
        let rndPos () = Position2(rndCoord (), rndCoord ())
        Photon(rndPos (), rndPos ())

    let BuildInitialGameState() =
        let planets = [
                Planet(Position2(0.5f, 0.5f));
                Planet(Position2(-0.5f, 0.5f));
                Planet(Position2(0.5f, -0.5f));
                Planet(Position2(-0.5f, -0.5f));
                ]
        let photons = [0..10000] |> List.map (fun _ -> buildRandomPhoton ())
        GameState(planets, photons)

