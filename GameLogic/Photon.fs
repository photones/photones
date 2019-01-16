#if INTERACTIVE
#I @"..\Bearded\libs"
#r @"amulware.Graphics.dll"
#r @"Bearded.Utilities.dll"
#r @"OpenTK.dll"
#endif

namespace GameLogic

open amulware.Graphics;
open Bearded.Utilities.SpaceTime
open OpenTK
open Utils
open Bearded.Utilities.Math

type public Photon(initialPos: Position2, initialSpeed: Velocity2, initialPointOfAttractionIndex: int) =
    let smallRandomVelocity () =
        let getNumber () = rndSingle () - 0.5f
        Velocity2(getNumber (), getNumber ()) * 0.1f
    
    let pointsOfAttraction = [
        Position2(0.5f,0.5f);
        Position2(0.5f,-0.5f);
        Position2(-0.5f,-0.5f);
        Position2(-0.5f,0.5f);
        ]

    let attractionRadius = 0.2f

    member this.Position: Position2 = initialPos
    member this.Speed: Velocity2 = initialSpeed
    member this.PointOfAttractionIndex: int = initialPointOfAttractionIndex
    member this.PointOfAttraction: Position2 = pointsOfAttraction.[this.PointOfAttractionIndex]

    member this.HasReachedPointOfAttraction () = (this.Position - this.PointOfAttraction).Length.NumericValue < attractionRadius


    member this.UpdatePhoton (elapsedTime: TimeSpan): Photon =
        let direction = this.PointOfAttraction - this.Position
        let acceleration = Acceleration2(direction.NumericValue) * 8.0f

        let velocity = this.Speed + acceleration * elapsedTime + smallRandomVelocity ()
        let maxspeed = 1.0f
        let cappedvelocity = if velocity.Length.NumericValue > maxspeed then Velocity2(velocity / velocity.Length * maxspeed) else velocity

        let position = this.Position + cappedvelocity * elapsedTime

        let pointOfAttractionIndex = if this.HasReachedPointOfAttraction () then (this.PointOfAttractionIndex + 1) % 4 else this.PointOfAttractionIndex
        Photon(position, cappedvelocity, pointOfAttractionIndex)


    interface IGameObject with
        member this.Position: Position2 = this.Position
        member this.Update(elapsedTime: TimeSpan): IGameObject = upcast this.UpdatePhoton elapsedTime
