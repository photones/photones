namespace GameLogic

open Bearded.Utilities.SpaceTime
open Utils
open amulware.Graphics

module public Photon =

    let private getRandomSpeed () = randomSingle () - 0.5f
    let private smallRandomVelocity () =
        Velocity2(getRandomSpeed (), getRandomSpeed ()) * 0.02f

    let private capVelocity maxSpeed (v: Velocity2) =
        if v.Length.NumericValue > maxSpeed then Velocity2(v.NumericValue.Normalized() * maxSpeed) else v

    let private capAccToGoal = capVelocity 0.1f
    let private capTotal = capVelocity 0.4f
        
    let private velocityToGoal (this : PhotonData) (elapsedTime: TimeSpan) =
        let pointOfAttraction = Position2(0.0f, 0.0f)
        let diff = pointOfAttraction - this.Position
        let acceleration =
            if diff.Length = Unit.Zero
            then Acceleration2(0.0f, 0.0f)
            else Acceleration2(diff.NumericValue.Normalized() * 1.0f)
        acceleration * elapsedTime

    let private interactionRadius = Unit(0.05f)

    let rec Update
            (tracer : Tracer) (this : PhotonData) (gameState : GameState) (updateArgs : UpdateEventArgs) = 

        let mutable alive = true
        let elapsed = updateArgs.ElapsedTimeInS

        // apply game of life like rules once per second
        let neighbors = gameState.TileMap.GetNeighbors this.Position interactionRadius
        match Seq.length neighbors with
        | t when t < 1 -> alive <- false
        | t when t < 5 -> gameState.Spawn (Photon (UpdatableState(this, Update)))
        | t when t < 100 -> ()
        | _ -> alive <- false

        let vToGoal = velocityToGoal this (TimeSpan(elapsed))
        let velocity =
            this.Speed
            + capAccToGoal vToGoal
            + (smallRandomVelocity ())
            |> capTotal
        let position = this.Position + velocity * (TimeSpan(elapsed))
        {
            Position = position;
            Speed = velocity;
            Alive = alive
        }

    let public CreatePhoton (data: PhotonData) = Photon (UpdatableState<PhotonData, GameState>(data, Update))
