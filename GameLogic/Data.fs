namespace GameLogic

open Bearded.Utilities.SpaceTime

type PhotonBehavior = Shy | Neutral | Aggressive

[<Struct>]
type public PhotonData = {
    Position: Position2;
    Velocity: Velocity2;
    Alive: bool;
    PlayerIndex: byte;
    Behavior: PhotonBehavior;
}

[<Struct>]
type public PlanetData = {
    Position: Position2;
    PlayerIndex: byte;
}


