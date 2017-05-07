namespace GameLogic

open Bearded.Utilities.SpaceTime
open OpenTK

[<AbstractClass>]
type GameObject() =
    member this.Position: Position2 = new Position2()

    abstract member Update : TimeSpan -> GameObject
