namespace GameLogic

module GameParameters =

    type public T = {
        /// The max number of interactions between photons that will be computed per frame.
        /// The higher the number, the better and smoother the interactions will be, but it comes at a
        /// cost of performance. If the interaction is programmed to move away from neighbors, and the
        /// neighbors are all on a horizontal line, you would want to move away from that line,
        /// orthogonally. However, what happens if this number is low and the interactionRadius is high,
        /// is that the average neighbor position will deviate horizontally significantly more than
        /// vertically. This means that we will move horizontally away instead of vertically.
        MaxPhotonInteractionsPerFrame: int;
    } 

    let defaultParameters = {
        MaxPhotonInteractionsPerFrame = 100
    }
