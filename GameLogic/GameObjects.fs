namespace GameLogic

open System.Collections.Generic
open System
open Bearded.Utilities.SpaceTime
open amulware.Graphics
open Utils

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

