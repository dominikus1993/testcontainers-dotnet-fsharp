namespace Testcontainers.FSharp

open DotNet.Testcontainers.Builders
open System

[<AutoOpen>]
module Container =
    type FsharpContainerBuilder internal () =
        member _.Yield() =
            ContainerBuilder()
              
        member _.Run(state: ContainerBuilder) = state.Build()

        [<CustomOperation("command")>]
        member _.Cmd (state: ContainerBuilder, commands: string[]) =
            state.WithCommand(commands)

        [<CustomOperation("images")>]
        member _.Metadata (state: ContainerBuilder, image: String) =
            state.WithImage(image)
    
    let container = FsharpContainerBuilder()

