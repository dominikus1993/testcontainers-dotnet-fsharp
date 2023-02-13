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
        command [|"/bin/sh"; "-c"; "trap : TERM INT; sleep infinity & wait"|]
    }
    
    alpine.Image.Name |> should equal "alpine"
    alpine.Image.Tag |> should equal "3.17"
    
    task {
        do! alpine.StartAsync()
        
        let! subject = alpine.ExecAsync([|"echo"; "Test"|], CancellationToken.None)
        
        subject.ExitCode |> should equal 0L
        subject.Stdout |> should haveSubstring "Test"
        do! alpine.StopAsync()
    }

[<Fact>]
let ``Test run localstack container`` () =
    let localstack = container {
        image "localstack/localstack:1.4.0"
        port 4566 true
        waitStrategy (Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(fun req -> req.ForPath("/_localstack/health").ForPort(4566us)))
    }
    
    task {
        do! localstack.StartAsync()
        
        let! subject = localstack.ExecAsync([|"awslocal"; "sqs"; "create-queue"; "--queue-name"; "sample-queue"|], CancellationToken.None)
       
        subject.ExitCode |> should equal 0L
        subject.Stdout |> should haveSubstring "http://localhost:4566/000000000000/sample-queue"
        do! localstack.StopAsync()
    }