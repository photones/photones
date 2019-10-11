namespace GameLogic

open Bearded.Utilities.SpaceTime
open amulware.Graphics

type public IPlayer =
    // abstract immutable property
    abstract member Id : byte 
    abstract member Color : Color 
    abstract member Target : Position2 

type PhotonBehavior = Shy | Neutral | Aggressive

[<Struct>]
type public PhotonData = {
    Position: Position2;
    Velocity: Velocity2;
    Size: Unit; // Radius of physical presence
    Alive: bool;
    Player: IPlayer;
    Behavior: PhotonBehavior;
}

[<Struct>]
type public PlanetData = {
    Position: Position2;
    Size: Unit; // Radius of physical presence
    Alive: bool;
    Player: IPlayer;
}


