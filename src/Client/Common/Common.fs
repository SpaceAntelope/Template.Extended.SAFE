namespace YourNamespace.Common


module Types =

    type NotificationText =
    | Info of string
    | Warning of string
    | Danger of string
    | Success of string
    | Empty

    type Msg =
    | PromiseFailed of exn
    | Notify of NotificationText
    | ToggleBusy of message:string option
    //| ReactError of exn * ReactErrorBoundary.InfoComponentObject

module TypeHelpers =
    open Types
    let BusyWithMsg = Some >> ToggleBusy
    let NotBusyMsg = None |> ToggleBusy


module Data =
    // pattern from https://fable.io/blog/Announcing-2-2.html -> #Fable.PowerPack
    open Thoth.Json
    open Fetch.Types

    let errorString (response: Response) =
        string response.Status + " " + response.StatusText + " for URL " + response.Url

    let inline fetchAs<'T> (url: string) (decoder: Decoder<'T>) (init: RequestProperties list) =
        GlobalFetch.fetch(RequestInfo.Url url, Fetch.requestProps init)
        |> Promise.bind (fun response ->
              if not response.Ok then
                  errorString response |> Error |> Promise.lift
              else
                  response.text() |> Promise.map (Decode.fromString decoder))
    let inline decoder() = Decode.Auto.generateDecoderCached(extra=(Extra.empty |> Extra.withInt64))

module View =
    open Fulma.Extensions.Wikiki
    open Fulma
    open Fable.React
    open Fable.React.Props
    open Fable.FontAwesome
    open Browser.Types
    open Browser

    let LoaderView (message : string option ) =
        let inline loader isBusy =
            PageLoader.pageLoader
                [ PageLoader.IsActive isBusy
                  PageLoader.Color IColor.IsBlack ]

        match message with
        | Some text ->
                loader true
                    [ Heading.h1 [] [str text ] ]
        | None ->
                loader false []


    let PageNotFound =
        div
            [ ClassName "animated bounceIn" ]
            [
                Notification.notification
                    [   Notification.Color IsDanger; ]
                    [
                        Heading.h1
                            [ Heading.Modifiers [Modifier.TextAlignment (Screen.All, TextAlignment.Left)]]
                            [   str "4"
                                Icon.icon [ Icon.Size IsLarge  ][ Fa.i [Fa.Regular.Compass; Fa.Spin] [] ]
                                str "4" ]
                        hr []
                        Heading.h4
                            [ Heading.Modifiers [Modifier.TextAlignment (Screen.All, TextAlignment.Right)]]
                            [ str "Page not found"]
                    ]
            ]

    let AddAnimation (animation: string) =
        Ref (fun (element:Element) ->
            if not (isNull element)
            then
                // element.addEventListener
                //     ("animationend", (fun e ->
                //         element.classList.remove "animated"
                //         element.classList.remove animation
                //     ) )
                element.classList.add "animated"
                element.classList.add animation)
                //Dom.console.info("[Ref]",element.classList))


