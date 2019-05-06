namespace YourNamespace.Home

open Fable.Core
open Browser.Types
open Elmish
open Browser
open Fulma
open Fable.React.ReactiveComponents
open Fable.Core
open Fable.React
open Browser.Types

module Types =

    type Model =
        { Data: string  }

    type Msg =
        | LoadData
        | DataLoaded of string

module State =
    open Types

    let init() = { Data = "" }, Cmd.ofMsg LoadData

    [<Emit("marked('$0')")>]
    let marked (text:string) : string = Util.jsNative

    [<Emit("new FileReader()")>]
    let getReader() : FileReader = Util.jsNative

    let update msg model =
        match msg with
        | LoadData ->
            let fileName = "../../../README.md"
            let readFile dispatch =
                let reader = getReader()
                reader.onload <- (fun e -> reader.result |> string |> marked |> DataLoaded |> dispatch )
                reader.readAsText(unbox fileName)

            model, Cmd.ofSub readFile

        | DataLoaded text ->
            let element = Dom.document.getElementById("ReadMeContent")
            element.innerHTML <- text
            { model with Data = text }, Cmd.none

// module View =


//     let root model dispatch =
//         [
//             script [ Src "https://cdn.jsdelivr.net/npm/marked/marked.min.js" ] []
//             Card.card [ ]
//                 [ Card.header [ ]
//                     [ Card.Header.title [ ]
//                         [ str "README.md" ]
//                       Card.Header.icon [ ]
//                         [ Fa.i [ Fa.Solid.AngleDown ] [] ] ] // ClassName "fa fa-angle-down" ] [ ] ] ]
//                   Card.content [ ]
//                     [ Content.content [ Id "ReadMeContent" ]
//                         [ str "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus nec iaculis mauris." ] ]
//                 //   Card.footer [ ]
//                 //     [ Card.Footer.a [ ]
//                 //         [ str "Save" ]
//                 //       Card.Footer.a [ ]
//                 //         [ str "Edit" ]
//                 //       Card.Footer.a [ ]
//                 //         [ str "Delete" ] ]
//                 ]
//         ]