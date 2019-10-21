(***hide***)
namespace GameLogic.Test

open System
open Xunit
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running
open Xunit.Abstractions
open GameLogic
open Bearded.Photones
open Bearded.Photones.Performance
open BenchmarkDotNet.Engines

module PerformanceOfTileMap =

(**
*)

    [<MemoryDiagnoser; MarkdownExporter>]
    [<SimpleJob(RunStrategy.ColdStart, targetCount = 1)>]
    type Bench() =

        let closeAfterNFrames n (g:PhotonesProgram) (e:BeardedUpdateEventArgs) =
            if (e.UpdateEventArgs.Frame > n) then g.Close()

        let evaluate framesToRun players =
            let game =
                new PhotonesProgram (
                    GameStateFactory.defaultScenario(players),
                    Action<PhotonesProgram,BeardedUpdateEventArgs>(closeAfterNFrames framesToRun)
                )

            game.Run()

        [<Params (2)>] 
        member val public Players = 0 with get, set

        [<Params (500)>] 
        member val public Frames = 0 with get, set

        [<Benchmark(Baseline=true)>]
        member this.TimeOfNFrames() =
            evaluate this.Frames this.Players

(** Now lets run the tests.  *)

    type TestRunner(output:ITestOutputHelper) =
        [<Fact>]
        let ``Run benchmarks`` () =
            BenchmarkRunner.Run<Bench>()

(**


***

The tests were run on a computer with the following specs.

<pre>
BenchmarkDotNet=v0.11.5, OS=Windows 10.0.18362
Intel Core i7-7820HQ CPU 2.90GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
  [Host]     : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 32bit LegacyJIT-v4.8.3752.0
  DefaultJob : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 32bit LegacyJIT-v4.8.3752.0
</pre>

*)