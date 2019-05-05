namespace YourNamespace
open Thoth.Json

// module FetchHelper =
//   open Fable.Core
//   open Fetch.Types
//   open Thoth.Json

//   // Custom error message
//   let errorString (response: Response) =
//       string response.Status + " " + response.StatusText + " for URL " + response.Url

//   let fetchWithDecoder<'T> (url: string) (decoder: Decoder<'T>) (init: RequestProperties list) =
//       GlobalFetch.fetch(RequestInfo.Url url, Fetch.requestProps init)
//       |> Promise.bind (fun response ->
//           if not response.Ok then
//               errorString response |> Error |> Promise.lift
//           else
//               response.text() |> Promise.map (Decode.fromString decoder))

//   // Inline the function so Fable can resolve the generic parameter at compile time
//   let inline fetchAs<'T> (url: string) (init: RequestProperties list) =
//       // In this example we use Thoth.Json cached auto decoders
//       // More info at: https://mangelmaxime.github.io/Thoth/json/v3.html#caching
//       let decoder = Decode.Auto.generateDecoderCached<'T>()
//       fetchWithDecoder url decoder init


module MainPage =
    open Browser
    open Fulma
    open Router
    open Shared
    open Elmish
    open Elmish.React

    open Fable.React
    open Fable.React.Props
    //open Fable.Fetch
    open Fable.Import

    open Thoth.Json

    open Fable.FontAwesome
    open Fable.FontAwesome.Free

    open Fetch.Types
    open Fable.Import

    type NotificationText =
    | Info of string
    | Warning of string
    | Danger of string
    | Success of string
    | Empty

    type Model = {
      Counter: Counter option;
      RemoteSources: RemoteSource list;
      Message: NotificationText
      CurrentPage: Page
      IsBurgerOpen: bool  }

    // The Msg type defines what events/actions can occur while the application is running
    // the state of the application changes *only* in reaction to these events
    type Msg =
    | Increment
    | Decrement
    | InitialCountLoaded of Result<Counter, string>
    | PromiseFailed of exn
    | RemoteSourcesLoaded of Result<RemoteSource list, string>
    | GenericFetch of Result<obj, string>
    | ToggleBurger

    let errorString (response: Response) =
        string response.Status + " " + response.StatusText + " for URL " + response.Url

    let inline fetchAs<'T> (url: string) (decoder: Decoder<'T>) (init: RequestProperties list) =
        GlobalFetch.fetch(RequestInfo.Url url, Fetch.requestProps init)
        //|> Promise.map (fun response -> response.text() |> Promise.map (Decode.fromString decoder))
        |> Promise.bind (fun response ->
              if not response.Ok then
                  errorString response |> Error |> Promise.lift
              else
                  response.text() |> Promise.map (Decode.fromString decoder))
    let inline decoder() = Decode.Auto.generateDecoder(extra=(Extra.empty |> Extra.withInt64))

    let decoder' = Decode.Auto.generateDecoderCached<RemoteSource list>()
    let initialCounter = fetchAs<Counter> "/api/init" <| decoder()
    let remoteSourceList = fetchAs<RemoteSource list> "api/RemoteSource" (Decode.Auto.generateDecoder(extra=(Extra.empty |> Extra.withInt64)))
    let testFetch = fetchAs<TestReturn> "api/RemoteSource/show/666" <| decoder()


    // let initialCounter = fetchAs<Counter> "/api/init"
    // let remoteSourceList = fetchAs<RemoteSource list> "api/RemoteSource"
    // let testFetch = fetchAs<TestReturn> "api/RemoteSource/show/666"


    let defaultModel = {
      Counter = None
      RemoteSources = []
      Message = Empty
      CurrentPage = TestPage
      IsBurgerOpen = false }

    let init () : Model * Cmd<Msg> =
        let loadCountCmd =
            Cmd.OfPromise.either
                initialCounter
                []
                (InitialCountLoaded)
                (PromiseFailed)
        let loadSourcesCmd =
            Cmd.OfPromise.either
                remoteSourceList
                []
                (RemoteSourcesLoaded)
                (PromiseFailed)

        // let testCmd =
        //     Cmd.OfPromise.either
        //         testFetch
        //         []
        //         (Ok >> (Result.map box) >> GenericFetch)
        //         (Error >> GenericFetch)

        defaultModel, Cmd.batch [
                            loadCountCmd
                            loadSourcesCmd
                            //testCmd
                            ]

    // The update function computes the next state of the application based on the current state and the incoming events/messages
    // It can also run side-effects (encoded as commands) like calling the server via Http.
    // these commands in turn, can dispatch messages to which the update function will react.
    let update (msg : Msg) (currentModel : Model) : Model * Cmd<Msg> =
        match currentModel.Counter, msg with
        | Some counter, Increment ->
            let nextModel = { currentModel with Counter = Some { Value = counter.Value + 1 } }
            nextModel, Cmd.none

        | Some counter, Decrement ->
            let nextModel = { currentModel with Counter = Some { Value = counter.Value - 1 } }
            nextModel, Cmd.none

        | _, InitialCountLoaded (Ok initialCount)->
            let nextModel = { currentModel with Counter = Some initialCount }
            nextModel, Cmd.none

        | _, RemoteSourcesLoaded (Ok sources) ->

            Dom.console.log(sources)
            { currentModel with RemoteSources = sources }, Cmd.none

        | _, RemoteSourcesLoaded (Error exn) ->
            Dom.console.log(exn)
            { currentModel with Message = Danger (exn) }, Cmd.none

        | _, GenericFetch (Ok payload) ->
            { currentModel with Message = Success (sprintf "%A" payload) }, Cmd.none

        | _, GenericFetch (Error exn) ->
            Dom.console.log(exn)
            { currentModel with Message = Danger (exn) }, Cmd.none

        | _, ToggleBurger ->

          { currentModel with IsBurgerOpen = not currentModel.IsBurgerOpen }, Cmd.none

        | _, PromiseFailed ex ->
            Dom.console.error(ex)
            { currentModel with Message = Danger (ex.Message) }, Cmd.none
        | _ -> currentModel, Cmd.none




    let safeComponents =
        let components =
            span [ ]
               [
                 a [ Href "https://saturnframework.github.io" ] [ str "Saturn" ]
                 str ", "
                 a [ Href "http://fable.io" ] [ str "Fable" ]
                 str ", "
                 a [ Href "https://elmish.github.io/elmish/" ] [ str "Elmish" ]
                 str ", "
                 a [ Href "https://fulma.github.io/Fulma" ] [ str "Fulma" ]
                 str ", "
                 a [ Href "https://dansup.github.io/bulma-templates/" ] [ str "Bulma\u00A0Templates" ]
               ]

        p [ ]
            [ strong [] [ str "SAFE Template" ]
              str " powered by: "
              components ]

    let show = function
    | { Counter = Some counter } -> string counter.Value
    | { Counter = None   } -> "Loading..."

    let navBrand isBurgerOpen dispatch =
        Navbar.Brand.div [ ]
            [ Navbar.Item.a
                [ Navbar.Item.Props [ Href "https://safe-stack.github.io/" ]
                  Navbar.Item.IsActive true ]
                [ img [ Src "https://safe-stack.github.io/images/safe_top.png"
                        Alt "Logo" ] ]
              Navbar.burger [ Fulma.Common.CustomClass (if isBurgerOpen then "is-active" else "")
                              Fulma.Common.Props [
                                        OnClick (fun _ -> dispatch ToggleBurger) ] ]
                        [ span [ ] [ ]
                          span [ ] [ ]
                          span [ ] [ ] ]
            ]

    let NavbarPageLinks =
        [   Router.Home, "Home"
            Router.TestPage, "Whatever"
            Router.RemoteLinks(Router.Index), "The List"
            Router.RemoteSources, "Sources" ]
        |> Router.navbarItems

    let NavbarDefaultLinks =
        [ //Navbar.Item.a [ ]
            //[ str "Home" ]
          Navbar.Item.a [ ]
            [ str "Examples" ]
          Navbar.Item.a [ ]
            [ str "Documentation" ]
          Navbar.Item.div [ ]
            [ Button.a
                [ Button.Color IsWhite
                  Button.IsOutlined
                  Button.Size IsSmall
                  Button.Props [ Href "https://github.com/SAFE-Stack/SAFE-template" ] ]
                [ Icon.icon [ ]
                    [ Fa.i [Fa.Brand.Github] [] ]
                  span [ ] [ str "View Source" ] ] ] ]
    let navMenu isBurgerOpen =
        Navbar.menu [ Navbar.Menu.IsActive isBurgerOpen ]
            [ Navbar.End.div [ ] (NavbarPageLinks @ NavbarDefaultLinks) ]

    let CounterBox (model : Model) (dispatch : Msg -> unit) =
        Box.box' [ ]
            [ Field.div [ Field.IsGrouped ]
                [ Control.p [ Control.IsExpanded ]
                    [ Input.text
                        [ Input.Disabled true
                          Input.Value (show model) ] ]
                  Control.p [ ]
                    [ Button.a
                        [ Button.Color IsPrimary
                          Button.OnClick (fun _ -> dispatch Increment) ]
                        [ str "+" ] ]
                  Control.p [ ]
                    [ Button.a
                        [ Button.Color IsPrimary
                          Button.OnClick (fun _ -> dispatch Decrement) ]
                        [ str "-" ] ] ] ]
    let ThinFooter children =
        Footer.footer
            [
                Modifiers    [
                    //Modifier.IsMarginless
                    //Modifier.IsPaddingless
                    Modifier.BackgroundColor Color.IsBlackTer
                    Modifier.TextSize  (Screen.All, TextSize.Is7 )
                    Modifier.TextAlignment (Screen.All, TextAlignment.Right )
                ]
                Props [ Style [Padding 3; PaddingRight 10 ] ]
            ]

            children

            //[ //Content.content
                //   [ Content.Modifiers [
                //       Modifier.TextAlignment (Screen.All, TextAlignment.Centered)
                //       Modifier.IsMarginless
                //       Modifier.IsPaddingless
                //       Modifier.TextColor IsWarning
                //       ] ]
                //   [ p [ ]
                //        [ str "Fulma" ]
                //     p [  Style [Color "red" ] ]
                //       [ str "A wrapper around Bulma to help you create application quicker" ] ]// ]



    let inline NotificationMessage message =
        match message with
        | Empty -> div [] []
        | Info text ->
            Notification.notification [ Notification.Color IsInfo ] [str text]
        | Warning text ->
            Notification.notification [ Notification.Color IsWarning ] [str text]
        | Danger text ->
            Notification.notification [ Notification.Color IsDanger ] [str text]
        | Success text ->
            Notification.notification [ Notification.Color IsSuccess ] [str text]

    let view (model : Model) (dispatch : Msg -> unit) =
        Hero.hero [ Hero.Color IsPrimary; Hero.IsFullHeight ]
            [ Hero.head [ ]
                [ Navbar.navbar [ ]
                    [ Container.container [ ]
                        [ navBrand model.IsBurgerOpen dispatch
                          navMenu model.IsBurgerOpen ] ] ]

              Hero.body [ ]
                [ Container.container [ Container.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ] ]
                    [   Column.column
                            [ Column.Width (Screen.All, Column.Is6)
                              Column.Offset (Screen.All, Column.Is3) ]
                            [ Heading.p [ ]
                                [ str "SAFE Template" ]
                              Heading.p [ Heading.IsSubtitle ]
                                [ safeComponents ]
                              CounterBox model dispatch ]

                        Column.column
                            [   Column.Width (Screen.All, Column.Is6)
                                Column.Offset (Screen.All, Column.Is3) ]
                            [ NotificationMessage model.Message ]

                        Column.column
                            [   Column.Width (Screen.All, Column.Is6)
                                Column.Offset (Screen.All, Column.Is3) ]
                            [   match model.RemoteSources with
                                | [] -> yield div [] [str "Resources not found"]
                                | sources -> yield RemoteSource.Views.Table sources
                            ]
                    ]
                ]

              ThinFooter [safeComponents]

            ]
