namespace GameLogic

open Bearded.Utilities.SpaceTime

type GameState(planets, photons) = 
    member this.Planets: Planet list = planets
    member this.Photons: Photon list = photons

    member this.Update(elapsedTime: TimeSpan): GameState =
        let planets = List.map (fun (p: Planet) -> p.UpdatePlanet(elapsedTime)) this.Planets
        let photons = List.map (fun (p: Photon) -> p.UpdatePhoton(elapsedTime)) this.Photons
        GameState(planets, photons)
