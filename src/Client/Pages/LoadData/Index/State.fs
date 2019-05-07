namespace YourNamespace.LoadData.Index

module State =
    open Elmish
    open Browser
    open YourNamespace.Common.Types
    open YourNamespace.Common.Data
    open YourNamespace.Common.TypeHelpers
    open Types

    let defaultModel =
        { Data = None }

    let init() =
        defaultModel,
        Cmd.ofMsg LoadData,
        Cmd.ofMsg (BusyWithMsg "Slowly loading data list...")

    let update (msg : Msg) (model : Model) : Model * Cmd<Msg> * Cmd<YourNamespace.Common.Types.Msg> =
        match msg with
        | LoadData ->
            let cmd =
                Cmd.OfPromise.either
                    Data.fetchRemoteDataIndex
                    []
                    (DataLoaded)
                    (UnexpectedError)

            model, cmd, Cmd.none

        | UnexpectedError err ->
            model, Cmd.none, Cmd.ofMsg (YourNamespace.Common.Types.Msg.PromiseFailed err)

        | DataLoaded (Ok sources) ->
            Dom.console.log(sources)
            match sources with
            | [] ->
                { model with Data = None }, Cmd.none, Cmd.ofMsg NotBusyMsg
            | data ->
                { model with Data = Some data }, Cmd.none, Cmd.ofMsg NotBusyMsg

        | DataLoaded (Error err) ->
            Dom.console.log(err)
            let cmd =
                NotificationText.Danger
                >> YourNamespace.Common.Types.Notify
                >> Cmd.ofMsg
                <| err

            model, Cmd.none, Cmd.batch[cmd;Cmd.ofMsg NotBusyMsg]