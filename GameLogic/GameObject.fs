namespace GameLogic

open System.Collections.Generic
open System
open Bearded.Utilities.SpaceTime

type GameObject<'GameState> =
    | Photon of UpdatableState<PhotonState, 'GameState>
    | Planet of UpdatableState<PlanetState, 'GameState>

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
    member this.Visit (visitPhoton : Action<PhotonState>, visitPlanet : Action<PlanetState>) : unit =
        match this with
        | Photon s -> visitPhoton.Invoke(s.State)
        | Planet s -> visitPlanet.Invoke(s.State)

