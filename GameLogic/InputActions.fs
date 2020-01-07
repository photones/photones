namespace GameLogic

open Bearded.Utilities.Input

/// This module contains all actions that can be bound by user input actions
module InputActions =

    type PlayerActions = {
        MoveVertical: IAction
        MoveHorizontal: IAction
    }

    type T = {
        ModGameSpeed: IAction
        ModA: IAction
        ModB: IAction
        ModC: IAction
        IntModD: IAction
        PlayerActions: PlayerActions
    }

