namespace YourNamespace.LoadData

open Microsoft.AspNetCore.Http
open FSharp.Control.Tasks.ContextInsensitive
open Saturn
open Shared
open System
open System.Threading.Tasks
open YourNamespace.LoadData

module Controller =
    let rnd = Random()

    let indexAction (ctx : HttpContext) =
        task {
            // Dramatic pause
            do! Task.Delay(2500)
            // match (rnd.Next() % 4) with
            // | 0 ->
            //     return raise
            //            <| Exception
            //                   ("This operation fails randomly 25% of the time, all the time.")

            // | _ -> return! Repository.FetchAll ""
            return! Repository.FetchAll ""
        }

    let showAction (ctx : HttpContext) (itemId : int) =
        task { return! Repository.FetchById itemId }

    let resource =
        controller {
            index indexAction
            show showAction
        }
