namespace GameLogic

open Bearded.Utilities.SpaceTime

type GameState(planets: Planet[]) = 
    member this.Planets: Planet[] = planets

    member this.Update(elapsedTime: TimeSpan): GameState =
        let planets = Array.map (fun (p: Planet) -> p.UpdatePlanet(elapsedTime)) this.Planets
        new GameState(planets)
