namespace GameLogic

open Bearded.Utilities.SpaceTime
open amulware.Graphics

[<Struct>]
type public PlayerData = {
    Id: byte;
    Color: Color;
    Target: Position2;
}

[<Struct>]
type public PlanetData = {
    Position: Position2;
    Size: Unit; // Radius of physical presence
    Alive: bool;
    PlayerId: byte;
}

type PhotonBehavior = Swarm

[<Struct>]
type public PhotonData = {
    Position: Position2;
    Velocity: Velocity2;
    Size: Unit; // Radius of physical presence
    Alive: bool;
    PlayerId: byte;
    Behavior: PhotonBehavior;
}


