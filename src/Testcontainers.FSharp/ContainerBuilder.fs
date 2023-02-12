namespace Testcontainers.FSharp

open System.Collections.Generic
open DotNet.Testcontainers.Builders
open System
open DotNet.Testcontainers.Configurations

[<AutoOpen>]
module Container =
    type FsharpContainerBuilder internal () =
        member _.Yield(_) =
            ContainerBuilder()
              
        member _.Run(state: ContainerBuilder) = state.Build()

        [<CustomOperation("command")>]
        member _.Cmd (state: ContainerBuilder, commands: string[]) =
            state.WithCommand(commands)
            
        [<CustomOperation("env")>]
        member _.Env (state: ContainerBuilder, envs: IReadOnlyDictionary<String, String>) =
            state.WithEnvironment(envs)

        [<CustomOperation("image")>]
        member _.Metadata (state: ContainerBuilder, image: String) =
            state.WithImage(image)
        
        [<CustomOperation("waitStrategy")>]
        member _.WaitUntil(state: ContainerBuilder, wait: IWaitForContainerOS) =
            state.WithWaitStrategy(wait)
    
    let container = FsharpContainerBuilder()

