namespace GameLogic

open amulware.Graphics;
open Bearded.Utilities.SpaceTime
open OpenTK

type Photon(initialPos: Position2, initialCenter: Position2) =
    inherit GameObject(initialPos)

    member this.Center: Position2 = initialCenter
    member this.Speed: float = 5.0

    member this.UpdatePhoton (elapsedTime: TimeSpan): Photon =
        let difference = this.Position - this.Center
        let rot = Matrix2.CreateRotation(this.Speed * elapsedTime.NumericValue |> float32);
        new Photon(this.Center + new Difference2(difference.NumericValue |> rot.Times), this.Center)

    override this.Update(elapsedTime: TimeSpan): GameObject = upcast this.UpdatePhoton elapsedTime
