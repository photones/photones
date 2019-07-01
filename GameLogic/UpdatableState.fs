namespace GameLogic

open Bearded.Utilities.SpaceTime

// TODO: move UpdateArgs into F# code, such that we don't have to add parameters for each new thing that we need
type public UpdatableState<'ObjectState, 'GameState> (initialState: 'ObjectState, update: 'ObjectState -> 'GameState -> TimeSpan -> TimeSpan -> 'ObjectState) =
    let mutable futureState : 'ObjectState = initialState
    let mutable currentState : 'ObjectState = initialState

    member this.State = currentState

    member this.Update (world : 'GameState) (elapsed : TimeSpan) (totalTime : TimeSpan) =
        futureState <- update currentState world elapsed totalTime

    member this.Refresh () =
        currentState <- futureState
