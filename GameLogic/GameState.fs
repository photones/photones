namespace GameLogic

open Bearded.Utilities.SpaceTime
open Bearded.Utilities
open System.Collections.Generic

[<Struct>]
type GameState(photons : Photon.T List) = 

    member this.Photons: Photon.T List = photons

    member this.Update(elapsedTime: TimeSpan): GameState =
        for i = 0 to photons.Count - 1 do
            photons.Item(i) <- (Photon.update elapsedTime (photons.Item(i)))
        GameState(photons)

