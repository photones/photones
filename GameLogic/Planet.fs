namespace GameLogic

open Bearded.Utilities.SpaceTime

type Planet(initialPos: Position2) =
    member this.Position: Position2 = initialPos
    member this.UpdatePlanet (elapsedTime: TimeSpan): Planet = this

    interface IGameObject with
        member this.Update(elapsedTime: TimeSpan): IGameObject = upcast this.UpdatePlanet elapsedTime
        member this.Position: Position2 = this.Position
