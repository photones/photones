#if INTERACTIVE
#r @"amulware.Graphics.dll"
#r @"Bearded.Utilities.dll"
#r @"OpenTK.dll"
#endif

namespace GameLogic

open Bearded.Utilities.SpaceTime
open Utils


module public Planet =
    [<Struct>]
    type public T =
        {Position: Position2; Size: single}

    let Update (this : T) (elapsed : TimeSpan) : T = 
        {Position = Position2.Zero; Size = 0.0f}

