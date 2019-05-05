namespace YourNamespace.LoadData.Show

module State =
    open Elmish
    open Browser
    open YourNamespace.Common.Types
    open YourNamespace.Common.Data
    open Types


    let BusyWithMsg = Some >> YourNamespace.Common.Types.ToggleBusy >> GlobalMsg
    let NotBusyMsg = None |> (YourNamespace.Common.Types.ToggleBusy >> GlobalMsg)

    let defaultModel = {
        Data = None }

    let init index =
        defaultModel,
        Cmd.batch [
            Cmd.ofMsg (BusyWithMsg "Loading item details...");
            Cmd.ofMsg (LoadData index) ]

    let update (msg : Msg) (model : Model) : Model * Cmd<Msg> =
        match msg with
        | LoadData index->
            let cmd =
                Cmd.OfPromise.either
                    (Data.fetchRemoteDataById index)
                    []
                    (DataLoaded)
                    (YourNamespace.Common.Types.Msg.PromiseFailed >> GlobalMsg)

            model, cmd

        | DataLoaded (Ok data) ->
            { model with Data = Some data }, Cmd.ofMsg NotBusyMsg

        | DataLoaded (Error err) ->
            Dom.console.log(err)
            let cmd =
                NotifyErrorMsg
                >> Cmd.ofMsg
                <| err

            model, Cmd.batch[cmd;Cmd.ofMsg NotBusyMsg]

        | _ ->
            model, Cmd.none
