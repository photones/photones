namespace GameLogic
open System.Text.RegularExpressions
open System

type Tracer(logger: Action<string>, setNrGameObjects: Action<int>) =
    member public this.Log msg = if not (Object.Equals(logger, null)) then logger.Invoke(msg)
    member public this.CountGameObjects nr =
        if not (Object.Equals(setNrGameObjects, null)) then setNrGameObjects.Invoke(nr)

module Utils =

    let private random = System.Random()
    let public randomSingle () = (single)(random.NextDouble())
    let public randomInt maxValue = random.Next(maxValue)

    /// See https://photones.github.io/photones/Performance%20of%20takeAtMost.html for a performance evaluation.
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
