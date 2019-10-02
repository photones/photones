(**
A Little Performance Test
=========================

I was interested to see which of two implementations of takeAtMost would be fastest.
The first to consider is the one we use in our code in
[Utils.fs](https://github.com/photones/photones/blob/master/GameLogic/Utils.fs).
The second is found below.
*)

module Tests
#load "../../GameLogic/Utils.fs"
open GameLogic.Utils

let takeAtMost1 n (sequence:seq<'T>) =
    let mutable i = n
    Seq.takeWhile (fun _ ->
        i <- i - 1
        i >= 0
        ) sequence |> Seq.toList

