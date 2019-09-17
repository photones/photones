namespace GameLogic

open Bearded.Utilities.SpaceTime
open Utils
open amulware.Graphics
open Microsoft.FSharp.Core
open Microsoft.FSharp.Collections
open OpenTK

module public Photon =

    let private getRandomSpeed () = randomSingle () - 0.5f
    let private smallRandomVelocity () = Velocity2(getRandomSpeed (), getRandomSpeed ()) * 0.02f

    let private capVelocity maxSpeed (v: Velocity2) =
        if v.Length.NumericValue > maxSpeed
        then Velocity2(v.NumericValue.Normalized() * maxSpeed)
        else v

    let private capToGoal = capVelocity 0.1f
    let private capTotal = capVelocity 0.4f
        
    let private velocityToGoal (this : PhotonData) (elapsedTime: TimeSpan) =
        let pointOfAttraction = Position2(0.0f, 0.0f)
        let diff = pointOfAttraction - this.Position
        let acceleration =
            if diff.Length = Unit.Zero
            then Acceleration2(0.0f, 0.0f)
            else Acceleration2(diff.NumericValue.Normalized() * 1.0f)
        capToGoal (acceleration * elapsedTime)

    let private interactionRadius = Unit(0.05f)
    let private collisionRadius = Unit(0.0005f)

    let private avgPosition (positions: seq<Position2>) =
        let sum = Seq.fold (fun total pos -> total + (Position2.Zero - pos)) Difference2.Zero positions
        let count = Seq.length positions
        sum * (single count) + Position2.Zero

    let filterPhotons objects = 
        seq{
        for n in objects do
            match n with
            | Planet _ -> ()
            | Photon d -> yield d.State
        }

    let getPosition (photon: PhotonData) = photon.Position

    let rec Update (tracer : Tracer) (this : PhotonData)
            (gameState : GameState) (updateArgs : UpdateEventArgs) = 

        let elapsedS = updateArgs.ElapsedTimeInS
        let elapsedT = TimeSpan(elapsedS)

        let mutable alive = true

        let collidingNeighbors = gameState.TileMap.GetNeighbors this.Position collisionRadius
        for state in filterPhotons collidingNeighbors do
            if state.PlayerIndex <> this.PlayerIndex then alive <- false

        let perturbation = smallRandomVelocity ()
        let vToGoal = velocityToGoal this elapsedT
        let velocity = this.Speed + vToGoal + perturbation |> capTotal
        let position = this.Position + velocity * elapsedT

        {
            Position = position;
            Speed = velocity;
            Alive = alive;
            PlayerIndex = this.PlayerIndex;
        }

    let public CreatePhoton (data: PhotonData) =
        Photon (UpdatableState<PhotonData, GameState>(data, Update))

