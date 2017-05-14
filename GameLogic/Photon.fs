namespace GameLogic

open amulware.Graphics;
open Bearded.Utilities.SpaceTime
open OpenTK

type Photon(initialPos: Position2) =
    inherit GameObject(initialPos)

    member this.Center: Position2 = Position2.Zero
    member this.Speed: float = 1.0

    member this.UpdatePhoton (elapsedTime: TimeSpan): Photon =
        let difference = this.Position - this.Center
        let rot = Matrix2.CreateRotation(this.Speed * elapsedTime.NumericValue |> float32);
        new Photon(this.Center + new Difference2(difference.NumericValue |> rot.Times))

    override this.Update(elapsedTime: TimeSpan): GameObject = upcast this.UpdatePhoton elapsedTime
