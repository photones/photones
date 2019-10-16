namespace GameLogic
open System.Text.RegularExpressions
open System
open System.Collections.Generic

type ITracer =
    abstract member Log : string -> unit
    abstract member CountGameObjects : int -> unit

type NullTracer() =
    interface ITracer with
        member this.Log _ = ()
        member this.CountGameObjects _ = ()

module Utils =

    let mutable public Tracer : ITracer = NullTracer() :> ITracer

    let private random = System.Random()
    let public randomSingle () = (single)(random.NextDouble())
    let public randomInt maxValue = random.Next(maxValue)

    let public bernoulli p = if random.NextDouble() < p then 1 else 0

    let readonly (l:List<'T>):IReadOnlyList<'T> = l :> IReadOnlyList<'T>

    /// See https://photones.github.io/photones/Performance%20of%20takeAtMost.html for a performance
    /// evaluation.
    let takeAtMost n (sequence:seq<'T>) = [
        let enumerator = sequence.GetEnumerator()
        let mutable i = n
        while i > 0 && enumerator.MoveNext() do
            yield enumerator.Current
            i <- i - 1
    ]

    let (|Regex|_|) pattern input =
        let m = Regex.Match(input, pattern)
        if m.Success then Some(List.tail [ for g in m.Groups -> g.Value ])
        else None
