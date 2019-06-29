namespace GameLogic

open System.Collections.Generic
open System
open Bearded.Utilities.SpaceTime

type GameState(gameObjects : GameObject List) = 

    member this.GameObjects: GameObject List = gameObjects

    member this.Update(elapsedTime: TimeSpan): unit =
        for o in this.GameObjects do
            o.Update elapsedTime
        for o in this.GameObjects do
            o.Refresh()

