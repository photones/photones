namespace GameLogic
open System
open Bearded.Utilities.SpaceTime

(**
We would like to exhaustively handle all different types of game objects in
various parts of the code. and make sure we don't miss cases as we add new
objects. In a regular OO language it is common to do this through a
[double dispatch construction](https://en.wikipedia.org/wiki/Visitor_pattern).
However, F# makes this virtually impossible to implement, because F# cannot
easily deal with circular references in the code. Luckily, in F# we can use
Discriminated Unions to do exhaustive pattern matches, so we don't need double
dispatch patterns for this.
*)

type public GameObject<'GameState> =
    | Photon of UpdatableState<PhotonData, 'GameState>
    | Planet of UpdatableState<PlanetData, 'GameState>

    member this.Alive =
        match this with
        | Photon s -> s.State.Alive
        | Planet s -> true

    member this.Position =
        match this with
        | Photon s -> s.State.Position
        | Planet s -> s.State.Position

    member this.Update (tracer:Tracer) (gameState:'GameState) (elapsedS:TimeSpan) =
        match this with
        | Photon s -> s.Update tracer gameState elapsedS
        | Planet s -> s.Update tracer gameState elapsedS

    member this.Refresh () =
        match this with
        | Photon s -> s.Refresh ()
        | Planet s -> s.Refresh ()

    /// Expose a visitor method to allow C# code to exhaustively match all possibilities
    member this.Visit (visitPhoton:Action<PhotonData>, visitPlanet:Action<PlanetData>):unit =
        match this with
        | Photon s -> visitPhoton.Invoke(s.State)
        | Planet s -> visitPlanet.Invoke(s.State)

