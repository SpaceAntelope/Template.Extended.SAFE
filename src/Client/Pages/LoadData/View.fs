namespace YourNamespace.LoadData


module View =
    open Browser
    open Fulma
    open Types
    open Fable.React
    open Fable.React.Props

    let root (model : Model) (dispatch : Msg -> unit) =
        Column.column
            [   //Column.Width (Screen.All, Column.Is6)
                Column.Width (Screen.All, Column.IsNarrow) ]
            [   yield
                    match model.IndexModel, model.ShowModel with
                    | Some indexModel, _ ->
                        Index.View.root indexModel (dispatch<<IndexMsg)

                    | _, Some showModel ->
                        Show.View.root showModel (dispatch<<ShowMsg)

                    | _ ->
                        YourNamespace.Common.View.PageNotFound
            ]