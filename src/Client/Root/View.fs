namespace YourNamespace.Root

module View =
    open Browser
    open Browser.Types
    open YourNamespace.Common.Types
    open YourNamespace.Common.View
    open YourNamespace.Root.Types
    open YourNamespace.Common
    open Fulma
    open Fable.React
    open Fable.React.Props
    open Fable.FontAwesome

    let SafeComponentLinks =
        p [ ]
            [ str "A "
              a [ Href "https://safe-stack.github.io/"]
                [ strong [] [ str "SAFE Template" ] ]
              str " extended by "
              a [ Href "https://areslazarus.com"] [ str "Ares Lazarus" ] ]

    let navBrand isBurgerOpen dispatch =
        Navbar.Brand.div [ ]
            [ Navbar.Item.a
                [ Navbar.Item.Props [ Href "https://safe-stack.github.io/" ]
                  Navbar.Item.IsActive true ]
                [ img [ Src "https://safe-stack.github.io/images/safe_top.png"
                        Alt "Logo" ] ]
              Navbar.burger [ Fulma.Common.CustomClass (if isBurgerOpen then "is-active" else "")
                              Fulma.Common.Props [
                                    Style [ Color IsLight ]
                                    OnClick (fun _ -> dispatch <| ToggleBurger (not isBurgerOpen)) ] ]
                        [ span [ ] [ ]
                          span [ ] [ ]
                          span [ ] [ ] ]
            ]

    let NavbarItems selectedPage (pages : (Router.Page * string) list) =
        [ for page, title in pages do
              yield  Navbar.Item.a
                          [
                              Navbar.Item.Props
                                [
                                    if selectedPage = page then
                                        yield Style [ FontWeight "bold"; BorderBottom "solid #00B89C 2px"; PaddingBottom 5.]

                                    yield OnClick(fun _ -> Router.modifyLocation page)
                                ] ]
                          [ ofString title ] ]

    let NavbarPageLinks selectedPage =
        Router.MainMenuLinks
        |> NavbarItems selectedPage

    let NavbarDefaultLinks =
        [
          Navbar.Item.div [ ]
            [ Button.a
                [ Button.Color IsWhite
                  Button.IsOutlined
                  Button.Size IsSmall
                  Button.Props [ Href "https://github.com/SpaceAntelope/Template.Extended.SAFE" ] ]
                [ Icon.icon [ ]
                    [ Fa.i [Fa.Brand.Github] [] ]
                  span [ ] [ str "View Source" ] ] ] ]

    let navMenu selectedPage isBurgerOpen =
        Navbar.menu [ Navbar.Menu.IsActive isBurgerOpen ]
            [ Navbar.End.div [ ] ((NavbarPageLinks selectedPage) @ NavbarDefaultLinks) ]

    let StatusBar children =
        Hero.foot
            [
                Modifiers    [
                    Modifier.BackgroundColor Color.IsBlackTer
                    Modifier.TextSize  (Screen.All, TextSize.Is7 )
                    Modifier.TextAlignment (Screen.All, TextAlignment.Right )
                ]
                Props [ Style [Padding 3; PaddingRight 10; PaddingLeft 10 ] ]
            ]

            children

    let inline NotificationMessage message dispatch =

        let slimStyle =
            Notification.Modifiers [
                Modifier.IsMarginless
                Modifier.IsPaddingless
                Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ]

        let content text =
            [
                Notification.delete
                    [   Modifiers
                            [   Modifier.IsPulledRight
                                Modifier.IsPaddingless
                                Modifier.IsMarginless ]
                        Props
                            [   Style [Top 0; Right 2]
                                OnClick (fun _ -> dispatch ClearNotification) ] ]
                    []

                str text
            ]

        match message with
        | NotificationText.Empty -> span [] []

        | NotificationText.Info text ->
            Notification.notification [ slimStyle; Notification.Color IsInfo ] (content text)

        | NotificationText.Warning text ->
            Notification.notification [slimStyle; Notification.Color IsWarning ] (content text)

        | NotificationText.Danger text ->
            Notification.notification [ slimStyle;Notification.Color IsDanger ] (content text)

        | NotificationText.Success text ->
            Notification.notification [ slimStyle;Notification.Color IsSuccess ] (content text)

    let split (str:string) = str.Split

    let errorView (model: Model) (dispath : Msg -> Unit) =
        Column.column
            [   Column.Width (Screen.Desktop, Column.Is6)
                Column.Width (Screen.Tablet, Column.Is6)
                Column.Width (Screen.Mobile, Column.Is10)
                Column.Offset (Screen.Desktop, Column.Is3)
                Column.Offset (Screen.Tablet, Column.Is3)
                Column.Offset (Screen.Mobile, Column.Is1)
                ]
            [
              div
                [ ClassName "animated bounceIn" ]
                [
                    Message.message [ Message.Color IsDanger ]
                        [ Message.header [ ]
                            [ str "React rendering appears to have imploded"
                              Delete.delete [ ] [ ] ]
                          Message.body [ ]
                            [  str "We apologize for the inconvenience \u2764" ]
                        ]
                ]
            ]

    let view model dispatch =
        Hero.hero [ Hero.Color IsPrimary; Hero.IsFullHeight ]
            [ Hero.head [ ]
                [ Navbar.navbar [ ]
                    [ Container.container [ ]
                        [ navBrand model.IsBurgerOpen dispatch
                          navMenu model.CurrentPage model.IsBurgerOpen ] ] ]

              Hero.body [ ]
                [
                    Container.container
                        [
                            Container.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ] ]
                        [
                            yield LoaderView model.BusyMessage

                            yield match model with
                            | { CurrentPage = Router.Counter
                                CounterModel = Some counterModel } ->
                                    YourNamespace.Counter.View.view counterModel (dispatch<<CounterMsg)

                            | { CurrentPage = Router.LoadData _
                                LoadDataModel = Some dataModel } ->
                                    YourNamespace.LoadData.View.root dataModel (dispatch<<LoadDataMsg)

                            | { CurrentPage = Router.About
                                AboutModel = Some homeModel } ->
                                    YourNamespace.About.View.root homeModel (dispatch<<AboutMsg)

                            | _ -> //{ CurrentPage = Router.Missing(_) } ->
                                    PageNotFound
                            |> YourNamespace.Common.ReactErrorBoundary.renderCatchFn
                                    (fun (error, info) ->
                                        Dom.console.error("SubComponent failed to render" + info.componentStack, error)
                                        YourNamespace.Common.Types.Msg.ReactError(error,info) |> (GlobalMsg>>dispatch ))
                                    (errorView model dispatch)
                        ]
                ]

              StatusBar [
                  div
                    [ Style [Display DisplayOptions.Flex] ]
                      [
                        div [Style [ FlexGrow 1; MarginRight 10 ] ]
                            [ NotificationMessage model.Message dispatch ]

                        SafeComponentLinks
                    ]
                ]
            ]

    let view' (model : Model) (dispatch : Msg -> unit) =
        div [ Class "is-main is-primary" ]
            [   Navbar.navbar [ ]
                    [ Container.container [ ]
                        [ navBrand model.IsBurgerOpen dispatch
                          navMenu model.CurrentPage model.IsBurgerOpen ] ]

                Container.container
                    [
                        Container.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ] ]
                    [
                        yield LoaderView model.BusyMessage

                        match model with
                        | { CurrentPage = Router.Counter
                            CounterModel = Some counterModel } ->
                                yield YourNamespace.Counter.View.view counterModel (dispatch<<CounterMsg)

                        | { CurrentPage = Router.LoadData _
                            LoadDataModel = Some dataModel } ->
                                yield YourNamespace.LoadData.View.root dataModel (dispatch<<LoadDataMsg)

                        | { CurrentPage = Router.About
                            AboutModel = Some homeModel } ->
                                yield YourNamespace.About.View.root homeModel (dispatch<<AboutMsg)
                                //yield div [] [str "Here's Home!"]

                        | _ -> //{ CurrentPage = Router.Missing(_) } ->
                                yield PageNotFound
                    ]


                StatusBar [
                  div
                    [ Style [Display DisplayOptions.Flex] ]
                      [
                        div [Style [ FlexGrow 1; MarginRight 10 ] ]
                            [ NotificationMessage model.Message dispatch ]

                        SafeComponentLinks
                    ]
                ]
            ]

