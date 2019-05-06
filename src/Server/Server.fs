namespace YourNamespace

open System.IO
open System.Threading.Tasks
open FSharp.Control.Tasks.V2
open Giraffe
open Saturn
open Shared
open Config
open Thoth.Json.Net

module Server =
    let tryGetEnv =
        System.Environment.GetEnvironmentVariable
        >> function
        | null
        | "" -> None
        | x -> Some x

    let publicPath = Path.GetFullPath "../Client/public"

    let port =
        "SERVER_PORT"
        |> tryGetEnv
        |> Option.map uint16
        |> Option.defaultValue 8085us

    let getInitCounter() : Task<Counter> = task { return { Value = 42 } }

    let webApp =
        router {
            get "/api/readme"
                (fun next ctx ->
                    task {
                        let text = File.ReadAllText("../../README.md").Replace("\r\n","\n")
                        return! json text next ctx
                    })

            get "/api/init"
                (fun next ctx -> task { let! counter = getInitCounter()
                                        return! json counter next ctx })

            (* /api/ tells webpack server to redirect to the server port *)
            forward "/api/LoadData" LoadData.Controller.resource
        }

    let app =
        application {
            url ("http://0.0.0.0:" + port.ToString() + "/")
            use_router webApp
            memory_cache
            use_static publicPath
            use_json_serializer
                (Thoth.Json.Giraffe.ThothSerializer
                     (extra = (Extra.empty |> Extra.withInt64)))
            use_gzip
            use_config
                (fun _ ->
                { ConnectionString =
                      if File.Exists "connection.info"
                      then Some <| File.ReadAllText("connection.info")
                      else None
                  })
        }

    run app
