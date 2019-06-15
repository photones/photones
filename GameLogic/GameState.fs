namespace GameLogic

open Bearded.Utilities.SpaceTime
open Bearded.Utilities
open System.Collections.Generic

[<Struct>]
type GameState(photons : Photon.T List) = 

    member this.Photons: Photon.T List = photons

    member this.Update(elapsedTime: TimeSpan): GameState =
        //let photons = List.map (Photon.update elapsedTime) this.Photons

        //let result = new List<Photon.T>()
        //for old in photons do
        //    result.Add(Photon.update elapsedTime old)
        //GameState(result)

        for i = 0 to photons.Count - 1 do
            photons.Item(i) <- (Photon.update elapsedTime (photons.Item(i)))
        GameState(photons)

