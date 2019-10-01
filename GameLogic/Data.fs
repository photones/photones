namespace GameLogic

open Bearded.Utilities.SpaceTime

[<Struct>]
type public PhotonData = {
    Position: Position2;
    Velocity: Velocity2;
    Alive: bool;
    PlayerIndex: byte;
}

[<Struct>]
type public PlanetData = {
    Position: Position2;
    PlayerIndex: byte;
}


