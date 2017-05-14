namespace GameLogic

open Bearded.Utilities.SpaceTime

type Planet(initialPos: Position2) =
    inherit GameObject(initialPos)

    member this.UpdatePlanet (elapsedTime: TimeSpan): Planet = this

    override this.Update(elapsedTime: TimeSpan): GameObject = upcast this.UpdatePlanet elapsedTime
