namespace YourNamespace.LoadData.Index

module View =
    open Fable.FontAwesome
    open YourNamespace.Common.Router
    open Fable.React
    open Fulma
    open Types

    let inline Row(idx, name) =
        tr []
            [ td
                []
                [
                    Fa.i [ Fa.Brand.Twitter ] [ ]
                    Button.a
                        [ Button.Props [ href (Page.LoadData(Show idx)) ]]
                        [ str name ]
                ]
            ]

    let inline Table(items : IdxStr list) =
        Table.table
            [ Table.IsHoverable ]
            [   thead []
                    [ tr []
                         [ th [] [ str "Choose an item to expand" ] ] ]
                tbody [] (List.map Row items) ]

    let root (model : Model) (dispatch : Msg -> unit) =
        Column.column
            [   //Column.Width (Screen.All, Column.Is6)
                Column.Width (Screen.All, Column.IsNarrow) ]
            [   match model.Data with
                | Some [] ->
                    yield div [] []
                | Some data ->
                    yield Table data
                | None ->
                    yield div [] [str "Data not available"]
            ]