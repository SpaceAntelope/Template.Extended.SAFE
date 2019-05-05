namespace YourNamespace.LoadData.Show
open System.Runtime.InteropServices

module View =
    open Shared
    open Fable.React
    open Fable.React.Props
    open Fulma
    open Types

    let embedded (item: RemoteData) =
        a [ ClassName item.ClassName
            Href item.EmbedReference
            Data ("theme", item.Theme) ]
          [  str ("Twits by or for " + item.Name)]

    let root (model : Model) (dispatch : Msg -> unit) =
        Column.column
            [   //Column.Width (Screen.All, Column.Is6)
                Column.Width (Screen.All, Column.IsNarrow) ]
            [   match model.Data with
                | Some data ->
                    yield embedded data
                    yield script
                        [   Async true
                            CharSet "utf-8"
                            Src "https://platform.twitter.com/widgets.js"]
                        []
                | sources ->
                    yield div [] []
            ]