namespace GameLogic

open Bearded.Utilities.SpaceTime

[<Struct>]
type public PhotonState =
    {Position: Position2; Speed: Velocity2; PoaIndex: int; Alive: bool}

[<Struct>]
type public PlanetState =
    {Position: Position2; Size: single}


