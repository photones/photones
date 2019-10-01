module Tests

open System
open Xunit

let takeAtMost1 n (sequence:seq<'T>) =
    let mutable i = n
    Seq.takeWhile (fun _ ->
        i <- i - 1
        i >= 0
        ) sequence |> Seq.toList

[<Fact>]
let ``My test`` () =
    Assert.True(true)
