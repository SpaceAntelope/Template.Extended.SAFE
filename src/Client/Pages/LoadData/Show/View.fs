namespace YourNamespace.LoadData.Show
open Fable.React.ReactiveComponents

module View =
    open Fable.Core
    open Browser
    open Shared
    open Fable.React
    open Fable.React.Props
    open Fulma
    open Types
    open Fable.FontAwesome

    [<Emit("if (twttr) twttr.widgets.load(document.getElementById($0))")>]
    let LoadTwitterWidgets (elementId: string) : unit = Util.jsNative

    let embedded (item: RemoteData) =
        a [ ClassName item.ClassName
            Href item.EmbedReference
            Data ("theme", item.Theme)
            Data ("height", 450)
            Ref (fun element ->
                if not (isNull element)
                then
                    LoadTwitterWidgets element.id
            ) ] [
                str (item.Name)
                Fa.i [
                    Fa.Props [ Style [ MarginLeft 10.] ]
                    Fa.Solid.SyncAlt
                    Fa.Spin ][]
            ]

    let root (model : Model) (dispatch : Msg -> unit) =
        Column.column [
            Column.Props [ Id "TwitterContainer" ]
            Column.Width (Screen.All, Column.IsNarrow) ] [
                match model.Data with
                | Some data ->
                    yield embedded data

                | _ ->
                    yield div [] []
            ]