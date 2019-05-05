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
