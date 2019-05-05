namespace YourNamespace.LoadData

module State =
    open Elmish
    open Browser
    open YourNamespace.Common.Router
    open Types


    let BusyWithMsg = Some >> YourNamespace.Common.Types.ToggleBusy >> GlobalMsg
    let NotBusyMsg = None |> (YourNamespace.Common.Types.ToggleBusy >> GlobalMsg)

    let defaultModel = {
        IndexModel = None
        ShowModel = None
    }

    let init page =
        match page with
        | Subsection.Index ->
            let (model, cmd) = Index.State.init()
            { defaultModel with IndexModel = Some model } , Cmd.map Types.IndexMsg cmd

        | Subsection.Show itemId ->
            let (model, cmd) = Show.State.init itemId
            { defaultModel with ShowModel = Some model } , Cmd.map Types.ShowMsg cmd

        // | 404?

    let update (msg : Msg) (model : Model) : Model * Cmd<Msg> =
        match msg, model with
        | IndexMsg msg, { IndexModel = Some indexModel } ->
            let (model', cmd) = Index.State.update msg indexModel
            { model with IndexModel = Some model'}, Cmd.map IndexMsg cmd

        | ShowMsg msg, { ShowModel = Some showModel } ->
            let (model', cmd) = Show.State.update msg showModel
            { model with ShowModel = Some model'}, Cmd.map ShowMsg cmd

        // | _ -> ??