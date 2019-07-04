namespace GameLogic

open Bearded.Utilities.SpaceTime

[<Struct>]
type public PhotonData =
    {Position: Position2; Speed: Velocity2; PointOfAttractionIndex: int; Alive: bool}

[<Struct>]
type public PlanetData =
    {Position: Position2; Size: single}


