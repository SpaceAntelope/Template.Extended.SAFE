namespace YourNamespace.LoadData.Index
open Fable.Import.RemoteDev
open Fulma

module View =
    open Fable.FontAwesome
    open YourNamespace.Common.Router
    open Fable.React
    open Fable.React.Props
    open Fulma
    open Types

    let inline Row(idx, name) =
        tr [] [
            td [] [
                Button.a [
                    Button.Option.IsFullWidth
                    Button.Props [ href (Page.LoadData(Show idx)) ] ] [
                        div [
                            Style [
                                Display DisplayOptions.Flex
                                AlignItems AlignItemsOptions.Center
                                Width "100%" ] ] [
                                    span [] [ str name ]
                                    span [ Style [ FlexGrow 1. ] ] []
                                    Fa.i [
                                        Fa.Brand.Twitter
                                        Fa.Props [ Style [ Color "#1DA1F2" ] ] ] []
                                    ]
                                ]
                            ]
                        ]


    let inline Table(items : IdxStr list) =
        Table.table [
            Table.IsHoverable
            Table.IsFullWidth
            Table.Props [ Style [ BorderRadius 15. ] ] ] [
                thead [] [
                    tr [] [
                        th [] [
                            str "Choose a timeline to expand" ] ] ]
                tbody [] (List.map Row items) ]


    let root (model : Model) (dispatch : Msg -> unit) =
        Column.column
            [] [
            match model.Data with
                | Some [] ->
                    yield div [] [ str "Data not available" ]
                | Some data ->
                    yield Table data
                | None ->
                    yield div [] []
            ]
