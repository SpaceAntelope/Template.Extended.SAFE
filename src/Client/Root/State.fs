namespace YourNamespace.Root

module State =
    open YourNamespace.Common.Types
    open YourNamespace.Root.Types
    open YourNamespace.Root.Data
    open YourNamespace.Common.Router
    open Browser
    open Shared
    open Elmish
    open Fable.Core

    let toggleBusyMsg = YourNamespace.Common.Types.ToggleBusy >> GlobalMsg

    let defaultModel = {
        CounterModel = None
        LoadDataModel = None
        BusyMessage = None
        Message = NotificationText.Info "Hello World! Here's a status message."
        CurrentPage = Counter
        IsBurgerOpen = false }

    let urlUpdate (result: Option<Page>) model =
        Dom.console.info(sprintf "[URL UPDATE] %A" result)

        match result with
        | None ->
            Dom.console.error("Error parsing url: " + window.location.href)

            model, modifyUrl model.CurrentPage

        | Some page ->
            let model = { model with CurrentPage = page }

            let collapsePageListCmd = Cmd.ofMsg <| ToggleBurger false

            let (model', cmd) =
                match page with
                | Page.Counter ->
                    let (model', cmd) = YourNamespace.Counter.State.init()
                    { model with CounterModel = Some model' }, Cmd.map CounterMsg cmd

                | Page.LoadData subPage ->
                    let (model', cmd) = YourNamespace.LoadData.State.init subPage
                    { model with LoadDataModel = Some model'}, Cmd.map LoadDataMsg cmd

                | _ -> model, Cmd.none

            model', Cmd.batch [collapsePageListCmd; cmd ]

    let init page =
        urlUpdate page defaultModel

    let update (msg : Msg) (model : Model) : Model * Cmd<Msg> =
        Dom.console.info("[EVENT]", sprintf "%A" msg)

        match msg with
        | GlobalMsg globalMsg ->
            match globalMsg with
            | YourNamespace.Common.Types.Msg.PromiseFailed ex ->
                { model with Message = NotificationText.Danger (ex.Message) }, Cmd.none

            | YourNamespace.Common.Types.Msg.Notify notification ->
                { model with Message = notification }, Cmd.none

            | YourNamespace.Common.Types.Msg.ToggleBusy message ->
                { model with BusyMessage = message }, Cmd.none

        | CounterMsg counterMsg ->
            match counterMsg, model.CounterModel with
            | YourNamespace.Counter.Types.Msg.GlobalMsg globalMsg, _ ->
                model, Cmd.ofMsg (GlobalMsg globalMsg)

            | _, Some counterModel ->
                let (model', cmd) = YourNamespace.Counter.State.update counterMsg counterModel
                { model with CounterModel = Some model'}, Cmd.map CounterMsg cmd
            | _ ->
                Dom.console.error("Received msg", msg, "but CounterModel is None")
                model, Cmd.none

        | LoadDataMsg dataMsg ->
            match dataMsg, model.LoadDataModel with
            | YourNamespace.LoadData.Types.Msg.GlobalMsg globalMsg, _ ->
                model, Cmd.ofMsg (GlobalMsg globalMsg)

            | _, Some dataModel ->
                let (model', cmd) = YourNamespace.LoadData.State.update dataMsg dataModel
                { model with LoadDataModel = Some model'}, Cmd.map LoadDataMsg cmd

            | _ ->
                Dom.console.error("Received msg", msg, "but SourceModel is None")
                model, Cmd.none

        | ToggleBurger state ->
            { model with IsBurgerOpen = state }, Cmd.none

        | ClearNotification ->
            { model with Message = NotificationText.Empty }, Cmd.none


