namespace GameLogic

open Bearded.Utilities.SpaceTime
open Bearded.Utilities
open System.Collections.Generic

type GameState(photons : Photon List) = 

    member this.Photons: Photon List = photons

    member this.Update(elapsedTime: TimeSpan): GameState =
        for i = 0 to photons.Count - 1 do
            photons.Item(i).Update(elapsedTime)
        for i = 0 to photons.Count - 1 do
            photons.Item(i).Refresh()
        GameState(photons)

