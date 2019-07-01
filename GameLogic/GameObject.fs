namespace GameLogic

open System.Collections.Generic
open System
open Bearded.Utilities.SpaceTime

type GameObject =
    | Photon of UpdatableState<PhotonState, IGameState>
    | Planet of UpdatableState<PlanetState, IGameState>

    member this.Alive =
        match this with
        | Photon s -> s.State.Alive
        | Planet s -> true

    member this.Position =
        match this with
        | Photon s -> s.State.Position
        | Planet s -> s.State.Position

    member this.Update (gameState : IGameState) (elapsed : TimeSpan) (totalTime : TimeSpan) =
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

    interface IGameObject with
        member this.Alive = this.Alive
        member this.Position = this.Position
        member this.Update (gameState : IGameState) (elapsed : TimeSpan) (totalTime : TimeSpan) = this.Update gameState elapsed totalTime
        member this.Refresh () = this.Refresh ()

