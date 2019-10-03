(**
A Little Performance Test
=========================

I was interested in seeing which of two implementations of takeAtMost would be fastest.
*)
(***hide***)
namespace GameLogic.Test

open Xunit
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running
open Xunit.Abstractions
open GameLogic

module PerformanceOfTakeAtMost =

(**
The first to consider is the one we use in our code in
[Utils.fs](https://github.com/photones/photones/blob/master/GameLogic/Utils.fs), reading
*)
    let takeAtMost n (sequence:seq<'T>) = [
        let enumerator = sequence.GetEnumerator()
        let mutable i = n
        while i > 0 && enumerator.MoveNext() do
            yield enumerator.Current
            i <- i - 1
    ]

(**
The second function uses a lambda function with a closure.
I would expect this one to be slower, and do more allocations therefore.
*)
    let takeAtMost1 n (sequence:seq<'T>) =
        let mutable i = n
        Seq.takeWhile (fun _ ->
            i <- i - 1
            i >= 0
            ) sequence |> Seq.toList


(**
We do a simple test whereby we evaluate the functions on a list of size n and arguments ranging from
1 to 2n.
*)

    [<MemoryDiagnoser; MarkdownExporter>]
    type Bench() =
        let mutable inputList = []
        let evaluate f =
            for n = 0 to inputList.Length * 2 do
                f n inputList |> ignore

        [<Params (100, 1000)>] 
        member val public ListSize = 0 with get, set

        [<GlobalSetup>]
        member this.GlobalSetupData() =
            inputList <- [for i = 1 to this.ListSize do yield i]

        [<Benchmark(Baseline=true)>]
        member this.TakeAtMost() =
            evaluate Utils.takeAtMost

        [<Benchmark>]
        member this.TakeAtMost1() =
            evaluate takeAtMost1

(** Now lets run the tests.  *)

    type TestRunner(output:ITestOutputHelper) =
        [<Fact>]
        let ``Run benchmarks`` () =
            BenchmarkRunner.Run<Bench>()

(**
|      Method | ListSize |        Mean |      Error |     Gen 0 | Gen 1 | Gen 2 | Allocated |
|------------ |--------- |------------:|-----------:|----------:|------:|------:|----------:|
|  TakeAtMost |      100 |    554.1 us |   6.613 us |   62.5000 |     - |     - |         - |
| TakeAtMost1 |      100 |    579.4 us |   9.091 us |   63.4766 |     - |     - |         - |
|  TakeAtMost |     1000 | 52,394.9 us | 957.084 us | 5700.0000 |     - |     - |         - |
| TakeAtMost1 |     1000 | 54,183.0 us | 650.906 us | 5700.0000 |     - |     - |         - |


As you can see, the difference is minimal, but our hypothesis seems to hold minimally. However, the
hypothesis that `takeAtMost1` would cause more allocations on the heap seems to be false.


***

The tests were run on a computer with the following specs.

<pre>
BenchmarkDotNet=v0.11.5, OS=Windows 10.0.18362
Intel Core i7-7820HQ CPU 2.90GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
  [Host]     : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 32bit LegacyJIT-v4.8.3752.0
  DefaultJob : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 32bit LegacyJIT-v4.8.3752.0
</pre>

*)