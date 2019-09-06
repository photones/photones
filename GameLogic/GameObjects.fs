(**
Game state, mutability and performance
======================================
*)
namespace GameLogic

open System
open amulware.Graphics

(**
We cannot work with an immutable gamestate, because this requires creating many new objects every frame,
which results in the garbage collector having to do a lot of work. 
This in turn result in unstable performance and very noticable stutters.

Therefore, we are bound to embrace mutability for our gamestate,
but we'd like a way to keep the mutability contained.
*)

type public UpdatableState<'ObjectState, 'GameState>
        (
            initialState: 'ObjectState,
            update: Tracer -> 'ObjectState -> 'GameState -> UpdateEventArgs -> 'ObjectState
        ) =
    let mutable futureState : 'ObjectState = initialState
    let mutable currentState : 'ObjectState = initialState

    member this.State = currentState

    member this.Update (tracer : Tracer) (world : 'GameState) (updateArgs : UpdateEventArgs) =
        futureState <- update tracer currentState world updateArgs

    member this.Refresh () =
        currentState <- futureState


(**
We would like to exhaustively handle all different types of game objects in various parts of the code.
In a regular OO language it is common to enable this through a
[double dispatch construction](https://en.wikipedia.org/wiki/Visitor_pattern).
However, F# makes this hard to implement due to all the circular references involved.
Luckily, F# being a functional language, it comes with a native construct to describe choice.
*)

type GameObject<'GameState> =
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

    member this.Update (tracer : Tracer) (gameState : 'GameState) (updateArgs : UpdateEventArgs) =
        match this with
        | Photon s -> s.Update tracer gameState updateArgs
        | Planet s -> s.Update tracer gameState updateArgs

    member this.Refresh () =
        match this with
        | Photon s -> s.Refresh ()
        | Planet s -> s.Refresh ()

    ///<summary>Expose a visitor method to allow C# code to exhaustively match all possibilities</summary>
    member this.Visit (visitPhoton : Action<PhotonData>, visitPlanet : Action<PlanetData>) : unit =
        match this with
        | Photon s -> visitPhoton.Invoke(s.State)
        | Planet s -> visitPlanet.Invoke(s.State)

