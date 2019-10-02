(**
A Little Performance Test
=========================

I was interested to see which of two implementations of takeAtMost would be fastest.
The first to consider is the one we use in our code in
[Utils.fs](https://github.com/photones/photones/blob/master/GameLogic/Utils.fs).
*)
#load "../../GameLogic/Utils.fs"
open GameLogic.Utils
open System.Diagnostics

let inputList = [1..1000]
let watch = new Stopwatch()

watch.Start()
for _ in [1..1000000] do
    takeAtMost 500 inputList |> ignore
watch.Stop()
(*** define-output: elapsed1 ***)
printf "%A" watch.Elapsed
(*** include-output: elapsed1 ***)

watch.Reset()

(**
The second function uses a lambda function with a closure.
This is expected to be slightly slower therefore.
*)

let takeAtMost1 n (sequence:seq<'T>) =
    let mutable i = n
    Seq.takeWhile (fun _ ->
        i <- i - 1
        i >= 0
        ) sequence |> Seq.toList

watch.Start()
for _ in [1..1000000] do
    takeAtMost1 500 inputList |> ignore
watch.Stop()
(*** define-output: elapsed2 ***)
printf "%A" watch.Elapsed
(*** include-output: elapsed2 ***)

(**
As you can see, the difference is minimal, but our hypothesis seems to hold.
Another thing to take into consideration is the fact that `takeAtMost1` probably causes more
allocations on the heap, thereby providing more work for the GC. This is a lot harder to measure,
and therefore left out of this exercise.
*)