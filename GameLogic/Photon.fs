namespace GameLogic

open amulware.Graphics;
open Bearded.Utilities.SpaceTime
open OpenTK

type public Photon(initialPos: Position2, initialCenter: Position2) =
    member this.Position: Position2 = initialPos
    member this.Center: Position2 = initialCenter
    member this.Speed: float = 5.0

    member this.UpdatePhoton (elapsedTime: TimeSpan): Photon =
        let difference = this.Position - this.Center
        let rot = Matrix2.CreateRotation(this.Speed * elapsedTime.NumericValue |> float32);
        Photon(this.Center + Difference2(difference.NumericValue |> rot.Times), this.Center)


    interface IGameObject with
        member this.Position: Position2 = this.Position
        member this.Update(elapsedTime: TimeSpan): IGameObject = upcast this.UpdatePhoton elapsedTime
