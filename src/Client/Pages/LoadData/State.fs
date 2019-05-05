namespace YourNamespace.LoadData

module State =
    open Elmish
    open Browser
    open YourNamespace.Common.Router
    open Types

    let defaultModel = {
        IndexModel = None
        ShowModel = None
    }

    let init page =
        match page with
        | Subsection.Index ->
            let (model, cmd, globalCmd) = Index.State.init()
            { defaultModel with IndexModel = Some model } , Cmd.map Types.IndexMsg cmd, globalCmd

        | Subsection.Show itemId ->
            let (model, cmd, globalCmd) = Show.State.init itemId
            { defaultModel with ShowModel = Some model } , Cmd.map Types.ShowMsg cmd, globalCmd

        // | 404?

    let update (msg : Msg) (model : Model) =
        match msg, model with
        | IndexMsg msg, { IndexModel = Some indexModel } ->
            let (model', cmd, globalCmd) = Index.State.update msg indexModel
            { model with IndexModel = Some model'}, Cmd.map IndexMsg cmd, globalCmd

        | ShowMsg msg, { ShowModel = Some showModel } ->
            let (model', cmd, globalCmd) = Show.State.update msg showModel
            { model with ShowModel = Some model'}, Cmd.map ShowMsg cmd, globalCmd

        // | _ -> ??