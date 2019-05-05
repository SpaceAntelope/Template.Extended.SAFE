namespace YourNamespace.LoadData.Index

module State =
    open Elmish
    open Browser
    open YourNamespace.Common.Types
    open YourNamespace.Common.Data
    open Types


    let BusyWithMsg = Some >> YourNamespace.Common.Types.ToggleBusy >> GlobalMsg
    let NotBusyMsg = None |> (YourNamespace.Common.Types.ToggleBusy >> GlobalMsg)

    let defaultModel =
        { Data = None }

    let init() =
        defaultModel,
        Cmd.batch [
            Cmd.ofMsg (BusyWithMsg "Loading data list...");
            Cmd.ofMsg LoadData ]

    let update (msg : Msg) (model : Model) : Model * Cmd<Msg> =
        match msg with
        | LoadData ->
            let cmd =
                Cmd.OfPromise.either
                    Data.fetchRemoteDataIndex
                    []
                    (DataLoaded)
                    (YourNamespace.Common.Types.Msg.PromiseFailed >> GlobalMsg)

            model, cmd

        | DataLoaded (Ok sources) ->
            Dom.console.log(sources)
            match sources with
            | [] ->
                { model with Data = None }, Cmd.ofMsg NotBusyMsg
            | data ->
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
