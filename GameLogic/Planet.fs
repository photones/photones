#if INTERACTIVE
#r @"amulware.Graphics.dll"
#r @"Bearded.Utilities.dll"
#r @"OpenTK.dll"
#endif

namespace GameLogic

open Bearded.Utilities.SpaceTime
open Utils

module public Planet =

    let update (this : PlanetData) (elapsed : TimeSpan) : PlanetData = 
        {Position = Position2.Zero; Size = 0.0f}

