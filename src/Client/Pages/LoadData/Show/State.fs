namespace YourNamespace.LoadData.Show
open YourNamespace.Common.Types

module State =
    open Elmish
    open Browser
    open YourNamespace.Common.TypeHelpers
    open Types

    let defaultModel = { Data = None }

    let init index =
        defaultModel,
        Cmd.ofMsg (LoadData index),
        Cmd.ofMsg (BusyWithMsg "Loading item details...")

    let update (msg : Msg) (model : Model) =
        match msg with
        | LoadData index->
            let cmd =
                Cmd.OfPromise.either
                    (Data.fetchRemoteDataById index)
                    []
                    (DataLoaded)
                    (UnexpectedError)

            model, cmd, Cmd.none

        | DataLoaded (Ok data) ->
            { model with Data = Some data }, Cmd.none, Cmd.ofMsg NotBusyMsg

        | DataLoaded (Error err) ->
            Dom.console.log(err)
            let cmd =
                NotificationText.Danger
                >> YourNamespace.Common.Types.Notify
                >> Cmd.ofMsg
                <| err

            model, Cmd.none, Cmd.batch[cmd;Cmd.ofMsg NotBusyMsg]

        | _ ->
            model, Cmd.none, Cmd.none
