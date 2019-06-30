namespace GameLogic

open Bearded.Utilities.SpaceTime

type IGameState =
    abstract member Update : TimeSpan -> unit
    abstract member GameObjects: seq<IGameObject>
    abstract member TileMap: ITileMap

and IGameObject =
    abstract member Position : Position2
    abstract member Update : IGameState -> TimeSpan -> unit
    abstract member Refresh : unit

and ITileMap =
    abstract member GetNeighbors : Position2 -> Unit -> seq<IGameObject>
