namespace GameLogic

open Bearded.Utilities.SpaceTime

[<Struct>]
type public PhotonState =
    {Position: Position2; Speed: Velocity2; PoaIndex: int; Alive: bool}

[<Struct>]
type public PlanetState =
    {Position: Position2; Size: single}


type IGameState =
    abstract member Update : TimeSpan -> TimeSpan -> unit
    abstract member GameObjects: seq<IGameObject>
    abstract member TileMap: ITileMap
    abstract member SpawnPhoton : PhotonState -> unit

and IGameObject =
    abstract member Position : Position2
    abstract member Update : IGameState -> TimeSpan -> TimeSpan -> unit
    abstract member Refresh : unit -> unit
    abstract member Alive : bool

and ITileMap =
    abstract member GetNeighbors : Position2 -> Unit -> seq<IGameObject>
