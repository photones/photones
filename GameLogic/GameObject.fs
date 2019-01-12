namespace GameLogic

open Bearded.Utilities.SpaceTime

type IGameObject =
    abstract member Position: Position2
    abstract member Update : TimeSpan -> IGameObject
