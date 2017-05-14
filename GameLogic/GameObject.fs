namespace GameLogic

open Bearded.Utilities.SpaceTime
open OpenTK

[<AbstractClass>]
type GameObject(pos: Position2) =
    member this.Position: Position2 = pos

    abstract member Update : TimeSpan -> GameObject
