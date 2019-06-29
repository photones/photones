namespace GameLogic

open System.Collections.Generic
open System
open Bearded.Utilities.SpaceTime

type GameObject =
    | Photon of UpdatableState<Photon.T, IGameState>
    | Planet of UpdatableState<Planet.T, IGameState>

    member this.Position =
        match this with
        | Photon s -> s.State.Position
        | Planet s -> s.State.Position

    member this.Update (gameState : IGameState) (elapsed : TimeSpan) =
        match this with
        | Photon s -> s.Update gameState elapsed
        | Planet s -> s.Update gameState elapsed

    member this.Refresh =
        match this with
        | Photon s -> s.Refresh
        | Planet s -> s.Refresh

    ///<summary>Expose a visitor method to allow C# code to exhaustively match all possibilities</summary>
    member this.Visit (visitPhoton : Action<Photon.T>, visitPlanet : Action<Planet.T>) : unit =
        match this with
        | Photon s -> visitPhoton.Invoke(s.State)
        | Planet s -> visitPlanet.Invoke(s.State)

