namespace GameLogic

open Bearded.Utilities.SpaceTime
open Bearded.Utilities

[<Struct>]
type GameState(photons : Photon.T list) = 

    member this.Photons: Photon.T list = photons

    member this.findNeighbors photon =
        let areNeighbors (p1: Photon.T) (p2: Photon.T) = (p1.Position - p2.Position).Length.NumericValue < 0.02f && Position2.op_Inequality(p1.Position, p2.Position)
        List.filter (areNeighbors photon) this.Photons

    member this.Update(elapsedTime: TimeSpan): GameState =
        //let neighborhoods = List.map this.findNeighbors this.Photons
        let neighborhoods = List.map (fun x -> []) this.Photons
        let photons = List.map (Photon.update elapsedTime) (List.zip this.Photons neighborhoods)
        GameState(photons)
