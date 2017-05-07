namespace GameLogic

open Bearded.Utilities.SpaceTime
open OpenTK

type Planet(initialPos: Vector2) =
    inherit GameObject()

    override this.Update(elapsedTime: TimeSpan): GameObject = upcast this
