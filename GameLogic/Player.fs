namespace GameLogic

open System
open Bearded.Utilities.SpaceTime
open amulware.Graphics
open Utils

type public Player(id:byte, color:Color) =

    let mutable _target = Position2.Zero

    let allPlanets (gameState:GameState) = 
        seq {
            for o in gameState.GameObjects do
                match o with
                | Planet d -> yield d.State
                | Photon _ -> ()
        }

    interface IUpdatetablePlayer<GameState> with 
        member this.Id = id
        member this.Color = color
        member this.Target = _target

        member this.Update (tracer:Tracer) (gameState:GameState) (elapsedS:TimeSpan) = 
            let planets = allPlanets gameState
            let hostilePlanets = planets |> Seq.filter (fun s -> s.Player <> (this :> IPlayer))
            _target <-
                if Seq.isEmpty hostilePlanets
                then Position2.Zero
                else (Seq.head hostilePlanets).Position
