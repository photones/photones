﻿(**
Game state, mutability and performance
======================================

We cannot work with an immutable gamestate, because this requires creating many new objects every frame,
which results in the garbage collector having to do a lot of work. 
This in turn result in unstable performance and very noticable stutters.
Therefore, we are bound to embrace mutability for our gamestate.

We isolate the use of mutability for game objects in the Update method of the following class.
This Update method has the possibility to change the futureState of the game object.
*)
namespace GameLogic

open amulware.Graphics

type public UpdatableState<'ObjectState, 'GameState>
        (
            initialState: 'ObjectState,
            update: Tracer -> 'ObjectState -> 'GameState -> UpdateEventArgs -> 'ObjectState
        ) =
    let mutable currentState : 'ObjectState = initialState
    let mutable futureState : 'ObjectState = initialState

    member this.State = currentState

    member this.Update (tracer : Tracer) (world : 'GameState) (updateArgs : UpdateEventArgs) =
        futureState <- update tracer currentState world updateArgs

    member this.Refresh () =
        currentState <- futureState


