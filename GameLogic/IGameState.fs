namespace GameLogic

open Bearded.Utilities.SpaceTime

type IGameState =
    abstract member Update : TimeSpan -> unit
    abstract member GameObjects: seq<IGameObject>

and IGameObject =
    abstract member Position : Position2
    abstract member Update : IGameState -> TimeSpan -> unit
    abstract member Refresh : unit
