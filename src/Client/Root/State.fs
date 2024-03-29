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
        AboutModel = None
        BusyMessage = None
        Message = NotificationText.Info "Hello World! Here's a status message."
        CurrentPage = Counter
        IsBurgerOpen = false
        ReactErrorModel = None }

    let initErrorModel ex info =
        {
            Exn = ex
            Info = info
            IsExpanded = false
        }

    let urlUpdate (result: Option<Page>) model =
        Dom.console.info(sprintf "[URL UPDATE] %A" result)

        let collapsePageListCmd =
            if model.IsBurgerOpen
            then
                Cmd.ofMsg <| ToggleBurger false
            else
                Cmd.none

        match result with
        | None ->
            let errMessage = window.location.href + " is not a valid url."

            Dom.console.error(errMessage)

            //{ model with Message = NotificationText.Warning errMessage }, Cmd.none//, modifyUrl model.CurrentPage
            { model with CurrentPage = Missing(NotFound404) }, collapsePageListCmd//, modifyUrl model.CurrentPage

        | Some page ->
            let model = { model with CurrentPage = page }

            let (model', cmd) =
                match page with
                | Page.Counter ->
                    let (model', cmd) = YourNamespace.Counter.State.init()
                    { model with CounterModel = Some model' }, Cmd.map CounterMsg cmd

                | Page.LoadData subPage ->
                    let (model', cmd, globalCmd) = YourNamespace.LoadData.State.init subPage
                    { model with LoadDataModel = Some model'},
                        Cmd.batch [
                            Cmd.map LoadDataMsg cmd
                            Cmd.map GlobalMsg globalCmd]

                | About ->
                    let (model', cmd) = YourNamespace.About.State.init()
                    { model with AboutModel = Some model'}, Cmd.map AboutMsg cmd

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
            | _, Some dataModel ->
                let (model', cmd, globalCmd) = YourNamespace.LoadData.State.update dataMsg dataModel
                { model with LoadDataModel = Some model'},
                    Cmd.batch [
                        Cmd.map LoadDataMsg cmd
                        Cmd.map GlobalMsg globalCmd]
            | _ ->
                Dom.console.error("Received msg", msg, "but LoadDataModel is None")
                model, Cmd.none

        | AboutMsg aboutMsg ->
            match aboutMsg, model.AboutModel with
            | _, Some homeModel ->
                let (model', cmd, globalCmd) = YourNamespace.About.State.update aboutMsg homeModel
                { model with AboutModel = Some model'},
                    Cmd.batch [
                        Cmd.map AboutMsg cmd
                        Cmd.map GlobalMsg globalCmd]

            | _ ->
                Dom.console.error("Received msg", msg, "but HomeModel is None")
                model, Cmd.none


        | ReactErrorMsg errorMsg ->
            match model.ReactErrorModel, errorMsg with
            | Some errorModel, IsReactErrorDetailsExpanded state ->
                { model with ReactErrorModel = Some { errorModel with IsExpanded = state } }, Cmd.none

            | _, ReactError (ex, info)  ->
                { model with ReactErrorModel = Some (initErrorModel ex info) }, Cmd.none

            | _ -> model, Cmd.none

        | ToggleBurger state ->
            { model with IsBurgerOpen = state }, Cmd.none

        | ClearNotification ->
            { model with Message = NotificationText.Empty }, Cmd.none

        | Reset ->
            init (Some model.CurrentPage)


