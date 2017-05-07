namespace GameLogic

open Bearded.Utilities.SpaceTime

type GameState(planets: Planet[]) = 
    member this.Planets: Planet[] = planets

    member this.Update(elapsedTime: TimeSpan) =
        this // TODO: map update on planets
