module Tests

open System.Threading
open DotNet.Testcontainers.Builders
open Testcontainers.FSharp
open Xunit
open FsUnit.Xunit

[<Fact>]
let ``Test alpine container`` () =
    let alpine = container {
        image "alpine:3.17"
        env readOnlyDict[("TEST", "2")]
    }
    
    alpine.Image.Name |> should equal "alpine"
    alpine.Image.Tag |> should equal "3.17"


[<Fact>]
let ``Test run alpine container`` () =
    let alpine = container {
        image "alpine:3.17"
        env readOnlyDict[("TEST", "2")]
        waitStrategy Wait.ForUnixContainer()
    }
    
    alpine.Image.Name |> should equal "alpine"
    alpine.Image.Tag |> should equal "3.17"
    
    task {
        do! alpine.StartAsync()
        
        let! subject = alpine.ExecAsync([|"echo"; "Test"|], CancellationToken.None)
        
        subject.ExitCode |> should equal 0
        subject.Stdout |> should equal "Test"
        do! alpine.StopAsync()
    }