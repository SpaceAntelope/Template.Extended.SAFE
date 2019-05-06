namespace YourNamespace.Home

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

    let init() = { Data = "" }, Cmd.ofMsg LoadData

    [<Emit("marked($0)")>]
    let marked (text:string) : string = Util.jsNative

    [<Emit("new FileReader()")>]
    let getReader() : FileReader = Util.jsNative
    
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

    [<Emit("marked($0)")>]
    let marked (text:string) : string = Util.jsNative

    let root (model: Types.Model) dispatch =
        Column.column
          [ Column.Width (Screen.All, Column.Is8)
            Column.Offset (Screen.All, Column.Is2) ]
          [
            script [ Src "https://cdn.jsdelivr.net/npm/marked/marked.min.js" ] []
            
            Card.card [ Props [ Style [ BorderRadius 10 ] ] ]
                [ Card.header [ ]
                    [ Card.Header.title [ ]
                        [ str "README.md" ]
                      Card.Header.icon [ ]
                        [ Fa.i [ Fa.Brand.Github ] [] ] // ClassName "fa fa-angle-down" ] [ ] ] ]
                    ]
                  Card.content [ Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Justified)] ]
                    [ Content.content 
                        [ Content.Props 
                            [ Ref (fun element ->
                                        if not (isNull element) && not (isNull model.Data)
                                        then element.innerHTML <- marked model.Data ) ] ] 
                        [ ] 
                    ]
                //   Card.footer [ ]
                //     [ Card.Footer.a 
                //         [ Href "https://github.com/SpaceAntelope/Template.Extended.SAFE/blob/master/README.md" ]
                //         [ str "https://github.com/SpaceAntelope/Template.Extended.SAFE/blob/master/README.md" ]
                      
                //         ]
                ]
          ]   