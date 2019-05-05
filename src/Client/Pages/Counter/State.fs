namespace YourNamespace.Counter
open System

module State =
    open YourNamespace.Counter.Types
    open YourNamespace.Counter.Data
    open Elmish

    let init() =

        let cmd =
            Cmd.OfPromise.either
                InitializeCounter
                []
                (InitialCountLoaded)
                (YourNamespace.Common.Types.Msg.PromiseFailed >> GlobalMsg)

        { Counter = None }, cmd

    let update (msg : Msg) (currentModel : Model) : Model * Cmd<Msg> =
        match currentModel.Counter, msg with
        | Some counter, Increment ->
            let nextModel =
                { currentModel with Counter = Some { Value = counter.Value + 1 } }
            nextModel, Cmd.none

        | Some counter, Decrement ->
            let nextModel =
                { currentModel with Counter = Some { Value = counter.Value - 1 } }
            nextModel, Cmd.none

        | _, InitialCountLoaded(Ok initialCount) ->
            let nextModel = { currentModel with Counter = Some initialCount }
            nextModel, Cmd.none

        | _, InitialCountLoaded(Error err) ->
            let cmd =
                Exception(err)
                |> YourNamespace.Common.Types.Msg.PromiseFailed
                |> GlobalMsg
                |> Cmd.ofMsg

            currentModel, cmd

        | _ -> currentModel, Cmd.none
