namespace YourNamespace.About

open Fable.Core
open Browser.Types
open Elmish
open Browser
open Fulma
open Fable.React.ReactiveComponents

open Fable.React
open Browser.Types
open Fable.Import
open Fable.Import.RemoteDev



module Types =

    type Model =
        { Data: string  }

    type Msg =
        | LoadData
        | DataLoaded of string
        | UnexpectedError of exn

module Data =
    open Fetch

    let fetchReadme =
        promise {
            let! response = fetch "api/readme" []
            let! text = response.text()
            return text
        } |> Promise.catch (fun ex -> ex.Message)

module State =
    open Types
    open Fable.Core

    [<Emit("marked($0)")>]
    let marked (text:string) : string = Util.jsNative

    let init() = { Data = "" }, Cmd.ofMsg LoadData

    let update msg model =
        match msg with
        | LoadData ->
            let cmd =
                Cmd.OfPromise.either
                    (fun _ -> Data.fetchReadme)
                    []
                    (DataLoaded)
                    (UnexpectedError)

            model, cmd, Cmd.none

        | DataLoaded text ->
            let result = JS.JSON.parse(text) :?> string
            { model with Data = marked result }, Cmd.none, Cmd.none

        | UnexpectedError err ->
            model, Cmd.none, Cmd.ofMsg (YourNamespace.Common.Types.Msg.PromiseFailed err)

module View =
    open Fable.React.Props
    open Fable.FontAwesome

    let root (model: Types.Model) dispatch =
        Column.column
          [ Column.Width (Screen.All, Column.Is8)
            Column.Offset (Screen.All, Column.Is2)
            Column.Props [ YourNamespace.Common.View.AddAnimation "fadeIn" ]
          ]
          [
            Card.card [ Props [ Style [ BorderRadius 10] ] ]
                [ Card.header [ ]
                    [ Card.Header.title [ ]
                        [ str "README.md" ]
                      Card.Header.icon [ ]
                        [ Fa.i [ Fa.Brand.Github ] [] ]
                    ]
                  Card.content
                    [
                        Modifiers
                            [   Modifier.TextAlignment (Screen.All, TextAlignment.Justified)
                                Modifier.IsPaddingless ]
                    ]
                    [ Content.content
                        [
                          Content.Modifiers [Modifier.BackgroundColor Color.IsLight]
                          Content.Props
                            [ Style [Height "80%"; MaxHeight 400.; Overflow "auto"; Padding 7.5 ]
                              Ref (fun element ->
                                        if not (isNull element) && not (isNull model.Data)
                                        then
                                            element.innerHTML <- model.Data
                                            Dom.console.info("[Ref]",element.classList)
                                            )
                            ]
                        ]
                        [ ]
                    ]
                  Card.footer [ ]
                    [ Card.Footer.a
                        [ Props [Href "https://github.com/SpaceAntelope/Template.Extended.SAFE/blob/master/README.md" ] ]
                        [ strong [ Style [Color IsLink] ] [ str "source" ] ]

                        ]
                ]
          ]