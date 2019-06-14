namespace GameLogic

open Bearded.Utilities.SpaceTime
open Bearded.Utilities

[<Struct>]
type GameState(photons : Photon.T list) = 

    member this.Photons: Photon.T list = photons

    member this.Update(elapsedTime: TimeSpan): GameState =
        let photons = List.map (Photon.update elapsedTime) this.Photons
        GameState(photons)
