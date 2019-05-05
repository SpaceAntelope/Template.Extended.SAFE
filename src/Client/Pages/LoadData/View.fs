namespace YourNamespace.LoadData

module View =
    open Shared
    open Fable.React
    open Fable.React.Props
    open Fulma
    open Types

    let root (model : Model) (dispatch : Msg -> unit) =
        Column.column
            [   //Column.Width (Screen.All, Column.Is6)
                Column.Width (Screen.All, Column.IsNarrow) ]
            [   match model.IndexModel, model.ShowModel with
                | Some indexModel, _ ->
                    yield Index.View.root indexModel (dispatch<<IndexMsg)

                | _, Some showModel ->
                    yield Show.View.root showModel (dispatch<<ShowMsg)
            ]