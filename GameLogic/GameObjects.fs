namespace GameLogic

open System.Collections.Generic
open System
open Bearded.Utilities.SpaceTime

// TODO: move UpdateArgs into F# code, such that we don't have to add parameters for each new thing that we need
type public UpdatableState<'ObjectState, 'GameState> (initialState: 'ObjectState, update: 'ObjectState -> 'GameState -> TimeSpan -> TimeSpan -> 'ObjectState) =
    let mutable futureState : 'ObjectState = initialState
    let mutable currentState : 'ObjectState = initialState

    member this.State = currentState

    member this.Update (world : 'GameState) (elapsed : TimeSpan) (totalTime : TimeSpan) =
        futureState <- update currentState world elapsed totalTime

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

    member this.Update (gameState : 'GameState) (elapsed : TimeSpan) (totalTime : TimeSpan) =
        match this with
        | Photon s -> s.Update gameState elapsed totalTime
        | Planet s -> s.Update gameState elapsed totalTime

    member this.Refresh () =
        match this with
        | Photon s -> s.Refresh ()
        | Planet s -> s.Refresh ()

    ///<summary>Expose a visitor method to allow C# code to exhaustively match all possibilities</summary>
    member this.Visit (visitPhoton : Action<PhotonData>, visitPlanet : Action<PlanetData>) : unit =
        match this with
        | Photon s -> visitPhoton.Invoke(s.State)
        | Planet s -> visitPlanet.Invoke(s.State)

