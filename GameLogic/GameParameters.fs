namespace GameLogic

module GameParameters =

    type public T = {
        /// The max number of interactions between photons that will be computed per frame. The
        /// higher the number, the better and smoother the interactions will be, but it comes at a
        /// cost of performance. If the interaction is programmed to move away from neighbors, and
        /// the neighbors are all on a horizontal line, you would want to move away from that line,
        /// orthogonally. However, what happens if this number is low and the interactionRadius is
        /// high, is that the average neighbor position will deviate horizontally significantly more
        /// than vertically. This means that we will move horizontally away instead of vertically.
        MaxPhotonInteractionsPerFrame: int
        /// To set the elapsed seconds per frame to a fixed number, make this setting nonzero.
        FixedElapsedSeconds: float
        /// To multiply the elapsed seconds per frame by some factor, make this setting nonzero.
        TimeModifier: float
        /// To cap the elapsed seconds per frame to some threshold, make this setting nonzero.
        MaxElapsedSeconds: float
        /// Some modulation parameters for dev testing
        ModA: single
        ModB: single
        ModC: single
        IntModD: int
    } 

    let defaultParameters = {
        MaxPhotonInteractionsPerFrame = 200
        FixedElapsedSeconds = 0.0050
        TimeModifier = 1.5
        MaxElapsedSeconds = 0.0000
        ModA = 0.0f
        ModB = 0.0f
        ModC = 0.0f
        IntModD = 0
    }

    type public T with
        // Make specific updates easily accessible in C#
        member this.WithTimeModifier (value: float) =
            {this with TimeModifier = value}
        member this.WithModA (value: single) =
            {this with ModA = value}
        member this.WithModB (value: single) =
            {this with ModB = value}
        member this.WithModC (value: single) =
            {this with ModC = value}
        member this.WithIntModD (value: int) =
            {this with IntModD = value}
