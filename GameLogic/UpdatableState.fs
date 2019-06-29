namespace GameLogic

open Bearded.Utilities.SpaceTime

type public UpdatableState<'State, 'World> (initialState: 'State, update: 'State -> 'World -> TimeSpan -> 'State) =
    let mutable futureState : 'State = initialState
    let mutable currentState : 'State = initialState

    member this.State = currentState

    member this.Update (world : 'World) (elapsed : TimeSpan) =
        futureState <- update currentState world elapsed

    member this.Refresh () =
        currentState <- futureState
