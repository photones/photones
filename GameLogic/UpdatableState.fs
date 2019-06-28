namespace GameLogic

open Bearded.Utilities.SpaceTime

type public UpdatableState<'State> (initialState: 'State, update: 'State -> TimeSpan -> 'State) =
    let mutable futureState : 'State = initialState
    let mutable currentState : 'State = initialState
    member this.Update (elapsed : TimeSpan) =
        futureState <- update currentState elapsed
    member this.Refresh () =
        currentState <- futureState
    member this.State = currentState

