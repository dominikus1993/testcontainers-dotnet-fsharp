module Tests

open Testcontainers.FSharp
open Xunit
open FsUnit.Xunit

[<Fact>]
let ``Test alpine container`` () =
    let alpine = container {
        cmd "echo"
    }
    
    alpine |> should not' null
