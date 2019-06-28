namespace GameLogic

open System.Collections.Generic
open System
open Bearded.Utilities.SpaceTime



type GameObject =
    | Photon of UpdatableState<Photon.T>
    | Planet of UpdatableState<Planet.T>

    member this.Update (elapsed : TimeSpan) =
        match this with
        | Photon s -> s.Update elapsed
        | Planet s -> s.Update elapsed

    member this.Refresh =
        match this with
        | Photon s -> s.Refresh
        | Planet s -> s.Refresh

    ///<summary>Expose a visitor method to allow C# code to exhaustively match all possibilities</summary>
    member this.Visit (visitPhoton : Action<Photon.T>, visitPlanet : Action<Planet.T>) : unit =
        match this with
        | Photon s -> visitPhoton.Invoke(s.State)
        | Planet s -> visitPlanet.Invoke(s.State)


type GameState(gameObjects : GameObject List) = 

    member this.GameObjects: GameObject List = gameObjects

    member this.Update(elapsedTime: TimeSpan): unit =
        for o in this.GameObjects do
            o.Update elapsedTime
        for o in this.GameObjects do
            o.Refresh()

