namespace GameLogic

open Bearded.Utilities.SpaceTime
open amulware.Graphics

type PhotonBehavior = Shy | Neutral | Aggressive

[<Struct>]
type public PhotonData = {
    Position: Position2;
    Velocity: Velocity2;
    Size: Unit; // Radius of physical presence
    Alive: bool;
    PlayerId: byte;
    Behavior: PhotonBehavior;
}

[<Struct>]
type public PlanetData = {
    Position: Position2;
    Size: Unit; // Radius of physical presence
    Alive: bool;
    PlayerId: byte;
}

[<Struct>]
type public PlayerData = {
    Id: byte;
    Color: Color;
    Target: Position2;
}


