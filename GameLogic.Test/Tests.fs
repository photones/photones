module Tests

open FsCheck.Xunit
open GameLogic.Utils

let takeAtMost1 n (sequence:seq<'T>) =
    let mutable i = n
    Seq.takeWhile (fun _ ->
        i <- i - 1
        i >= 0
        ) sequence |> Seq.toList

[<Property>]
let ``Test takeAtMost against alternative implementation``(n:int, xs:list<int>) =
    takeAtMost n xs = takeAtMost1 n xs

