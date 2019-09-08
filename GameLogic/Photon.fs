#if INTERACTIVE
#r @"amulware.Graphics.dll"
#r @"Bearded.Utilities.dll"
#r @"OpenTK.dll"
#endif

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
        
    let private attractionRadius = 0.2f

    let PointsOfAttraction = [
        Position2(-0.1f,0.3f);
        Position2(0.6f,0.1f);
        Position2(0.0f,-0.6f);
        Position2(-0.3f,0.6f);
        ]

    let private pointOfAttraction (this : PhotonData) : Position2 =
        PointsOfAttraction.[this.PointOfAttractionIndex]
    let private hasReachedPointOfAttraction (this : PhotonData) =
        Unit.op_LessThan((this.Position - pointOfAttraction this).Length, Unit(attractionRadius))

    let private velocityToGoal (this : PhotonData) (elapsedTime: TimeSpan) =
        let diff = pointOfAttraction this - this.Position
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
        let totalTime = updateArgs.TimeInS

        // apply game of life like rules once per second
        if (totalTime - (totalTime |> int |> float)) < elapsed then
            let neighbors = gameState.TileMap.GetNeighbors this.Position interactionRadius
            match Seq.length neighbors with
            | t when t < 2 -> alive <- false
            | t when t < 20 -> gameState.Spawn (Photon (UpdatableState(this, Update)))
            | t when t < 200 -> ()
            | _ -> alive <- false

        let vToGoal = velocityToGoal this (TimeSpan(elapsed))
        let velocity =
            this.Speed
            + capAccToGoal vToGoal
            + (smallRandomVelocity ())
            |> capTotal
        let position = this.Position + velocity * (TimeSpan(elapsed))
        let pointOfAttractionIndex =
            if hasReachedPointOfAttraction this
            then (this.PointOfAttractionIndex + 1) % PointsOfAttraction.Length
            else this.PointOfAttractionIndex
        {
            Position = position;
            Speed = velocity;
            PointOfAttractionIndex = pointOfAttractionIndex;
            Alive = alive
        }

    let public CreatePhoton (data: PhotonData) = Photon (UpdatableState<PhotonData, GameState>(data, Update))
