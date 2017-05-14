namespace GameLogic

open Bearded.Utilities.SpaceTime

type GameState(planets, photons) = 
    member this.Planets: Planet[] = planets
    member this.Photons: Photon[] = photons

    member this.Update(elapsedTime: TimeSpan): GameState =
        let planets = Array.map (fun (p: Planet) -> p.UpdatePlanet(elapsedTime)) this.Planets
        let photons = Array.map (fun (p: Photon) -> p.UpdatePhoton(elapsedTime)) this.Photons
        new GameState(planets, photons)
