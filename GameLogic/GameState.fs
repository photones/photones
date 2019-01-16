namespace GameLogic

open Bearded.Utilities.SpaceTime

type GameState(photons) = 
    member this.Photons: Photon list = photons

    member this.Update(elapsedTime: TimeSpan): GameState =
        let photons = List.map (fun (p: Photon) -> p.UpdatePhoton(elapsedTime)) this.Photons
        GameState(photons)
