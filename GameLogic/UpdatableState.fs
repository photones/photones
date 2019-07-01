namespace GameLogic

open Bearded.Utilities.SpaceTime

// TODO: move UpdateArgs into F# code, such that we don't have to add parameters for each new thing that we need
type public UpdatableState<'State, 'World> (initialState: 'State, update: 'State -> 'World -> TimeSpan -> TimeSpan -> 'State) =
    let mutable futureState : 'State = initialState
    let mutable currentState : 'State = initialState

    member this.State = currentState

    member this.Update (world : 'World) (elapsed : TimeSpan) (totalTime : TimeSpan) =
        futureState <- update currentState world elapsed totalTime

    member this.Refresh () =
        currentState <- futureState
