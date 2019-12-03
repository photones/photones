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
Preliminary investigations suggested that the computation of photon interactions form a performance
bottleneck. We have added a game parameter to bound the number of interactions computed per frame
per photon. The neighborhood of each photon is randomly sampled with a samplesize of at most the
given threshold. The idea behind this is that across several frames the resulting behaviour of the
photons will closely approximate the original behaviour.

To actually verify that limiting the number of interactions being computed per photon improve the
performance, we conduct a benchmarking experiment, varying the interaction threshold. This test
currently only involves one planet continuously generating photons. We found that adding more
players has a significant negative impact on the predictability of the number of game objects in the
game, which in turn has a negative impact on the predictability of the workload in each case. To
make the workload comparable across the various test cases we only consider one player for now.
*)

    [<MemoryDiagnoser; MarkdownExporter>]
    [<SimpleJob(RunStrategy.ColdStart, targetCount = 10)>]
    type Bench() =

        let closeAfterNFrames n (g:PhotonesProgram) (e:BeardedUpdateEventArgs) =
            if (e.UpdateEventArgs.Frame > n) then g.Close()

        let evaluate framesToRun players interactions =
            let gameParameters = {
                GameParameters.T.MaxPhotonInteractionsPerFrame = interactions;
                GameParameters.T.FixedElapsedSeconds = 0.02;
                GameParameters.T.TimeModifier = 0.0;
                GameParameters.T.MaxElapsedSeconds = 0.0;
            }
            let game =
                new PhotonesProgram (
                    GameStateFactory.defaultScenario gameParameters players,
                    Action<PhotonesProgram,BeardedUpdateEventArgs>(closeAfterNFrames framesToRun)
                )

            game.Run()

        [<Params (1)>] 
        member val public Players = 0 with get, set

        [<Params (1000)>] 
        member val public Frames = 0 with get, set

        [<Params (1, 5, 10, 50, 100)>] 
        member val public Interactions = 0 with get, set

        [<Benchmark()>]
        member this.TimeOfNFrames() =
            evaluate this.Frames this.Players this.Interactions

(** Now lets run the tests.  *)

    type TestRunner(output:ITestOutputHelper) =
        [<Fact>]
        let ``Run benchmarks`` () =
            BenchmarkRunner.Run<Bench>()

(**

|           Method|Players| Frames | Interactions|     Mean |    StdDev | Allocated |
|-----------------|-------|------- |-------------|---------:|----------:|----------:|
|**TimeOfNFrames**| **1** |**1000**|  **1**      | **9.678 s**|**0.2550 s**|**2.73 GB**|
|**TimeOfNFrames**| **1** |**1000**|  **5**      |**11.633 s**|**0.1762 s**|**3.12 GB**|
|**TimeOfNFrames**| **1** |**1000**| **10**      |**13.585 s**|**0.2891 s**|**3.51 GB**|
|**TimeOfNFrames**| **1** |**1000**| **50**      |**16.129 s**|**0.2445 s**|**4.25 GB**|
|**TimeOfNFrames**| **1** |**1000**|**100**      |**16.176 s**|**0.1051 s**|**4.29 GB**|

We can definitely see a correlation between the running time and the number of interactions per
photon. The impact is not as big as we had hoped though. We also see a big difference in allocations
being done. This is not something I expected to see, and may be worth investigating further.

***

The tests were run on a computer with the following specs.

<pre>
BenchmarkDotNet=v0.11.5, OS=Windows 10.0.18362
Intel Core i7-7820HQ CPU 2.90GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
  [Host]     : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 32bit LegacyJIT-v4.8.3752.0
  DefaultJob : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 32bit LegacyJIT-v4.8.3752.0
</pre>

*)