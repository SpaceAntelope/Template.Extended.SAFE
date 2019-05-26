namespace YourNamespace.Root

module View =
    open Browser
    open Browser.Types
    open YourNamespace.Common.Types
    open YourNamespace.Common.View
    open YourNamespace.Root.Types
    open YourNamespace.Common
    open Fulma
    open Fulma.Extensions.Wikiki
    open Fable.React
    open Fable.React.Props
    open Fable.FontAwesome

    let SafeComponentLinks =
        p []
            [ str "A "
              a [ Href "https://safe-stack.github.io/" ]
                [ strong [] [ str "SAFE Template" ] ]
              str " extended by "
              a [ Href "https://areslazarus.com" ] [ str "Ares Lazarus" ] ]

    let navBrand isBurgerOpen dispatch =
        Navbar.Brand.div []
            [ Navbar.Item.a
                [ Navbar.Item.Props [ Href "https://safe-stack.github.io/" ]
                  Navbar.Item.IsActive true ]
                [ img [ Src "https://safe-stack.github.io/images/safe_top.png"
                        Alt "Logo" ] ]
              Navbar.burger [ Fulma.Common.CustomClass(if isBurgerOpen then "is-active" else "")
                              Fulma.Common.Props [
                                    Style [ Color IsLight ]
                                    OnClick (fun _ -> dispatch <| ToggleBurger(not isBurgerOpen)) ] ]
                        [ span [] []
                          span [] []
                          span [] [] ]
            ]

    let NavbarItems selectedPage (pages : (Router.Page * string) list) =
        [ for page, title in pages do
              yield Navbar.Item.a
                          [
                                if selectedPage = page then
                                    yield Navbar.Item.IsActive true

                                yield Navbar.Item.Props
                                   [
                                            //yield Style [ FontWeight "bold"; BorderBottom "solid #00B89C 2px"; PaddingBottom 5.]

                                        yield OnClick (fun _ -> Router.modifyLocation page)
                                    ] ]
                          [ ofString title ] ]

    let NavbarPageLinks selectedPage =
        Router.MainMenuLinks
        |> NavbarItems selectedPage

    let NavbarDefaultLinks =
        [
          Navbar.Item.div []
            [ Button.a
                [ Button.Color IsWhite
                  Button.IsOutlined
                  Button.Size IsSmall
                  Button.Props [ Href "https://github.com/SpaceAntelope/Template.Extended.SAFE" ] ]
                [ Icon.icon []
                    [ Fa.i [ Fa.Brand.Github ] [] ]
                  span [] [ str "View Source" ] ] ] ]

    let navMenu selectedPage isBurgerOpen =
        Navbar.menu [ Navbar.Menu.IsActive isBurgerOpen ]
            [ Navbar.End.div [] ((NavbarPageLinks selectedPage) @ NavbarDefaultLinks) ]

    let StatusBar children =
        Hero.foot
            [
                Modifiers [
                    Modifier.BackgroundColor Color.IsBlackTer
                    Modifier.TextSize(Screen.All, TextSize.Is7)
                    Modifier.TextAlignment (Screen.All, TextAlignment.Right)
                ]
                Props [ Style [ Padding 3; PaddingRight 10; PaddingLeft 10 ] ]
            ]

            children

    let inline NotificationMessage message dispatch =

        let slimStyle =
            Notification.Modifiers [
                Modifier.IsMarginless
                Modifier.IsPaddingless
                Modifier.TextAlignment(Screen.All, TextAlignment.Centered) ]

        let content text =
            [
                Notification.delete [
                    Modifiers [
                        Modifier.IsPulledRight
                        Modifier.IsPaddingless
                        Modifier.IsMarginless ]
                    Props [
                        Style [ Top 0; Right 2 ]
                        OnClick(fun _ -> dispatch ClearNotification) ] ]
                    []

                str text
            ]

        match message with
        | NotificationText.Empty -> span [] []

        | NotificationText.Info text ->
            Notification.notification [ slimStyle; Notification.Color IsInfo ] (content text)

        | NotificationText.Warning text ->
            Notification.notification [ slimStyle; Notification.Color IsWarning ] (content text)

        | NotificationText.Danger text ->
            Notification.notification [ slimStyle; Notification.Color IsDanger ] (content text)

        | NotificationText.Success text ->
            Notification.notification [ slimStyle; Notification.Color IsSuccess ] (content text)

    let textToLines (text:string) =
        text.Split('\n')
        |> List.ofArray
        |> List.map (fun line -> div [] [str line])

    let errorMessage errModel dispatch =
        Message.message [
            Message.Color IsDanger
            Message.Props [
                AddAnimation "bounceIn"]][
                    Message.header [] [
                        str "React rendering appears to have imploded"
                        Delete.delete [ Delete.OnClick (fun e -> dispatch Reset)] [] ]

                    Message.body [][
                        yield Text.p [
                                Modifiers [
                                    Modifier.TextSize (Screen.All, TextSize.Is4)
                                    Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ]] [
                                    str <| errModel.Exn.Message.Replace(errModel.Info.componentStack,"") ]

                        yield Text.p [
                            Props[OnClick (fun e -> dispatch<<ReactErrorMsg<<IsReactErrorDetailsExpanded <| not errModel.IsExpanded )]][
                                a [] [
                                    span [Style[Margin 5]][str "Details"]
                                    Fa.i [
                                        yield if not errModel.IsExpanded
                                            then Fa.Solid.AngleDown
                                            else Fa.Solid.AngleLeft][] ] ]

                        if errModel.IsExpanded
                        then
                            yield Content.content [
                                    Content.Modifiers [
                                        Modifier.BackgroundColor Color.IsLight
                                        Modifier.TextColor IsGreyDarker
                                        Modifier.TextAlignment (Screen.All, TextAlignment.Left)]

                                    Content.Props [
                                        AddAnimation "fadeIn"
                                        Style [
                                            Padding 10
                                            OverflowY "auto"
                                            BoxShadow "5px 5px 8px #888888"
                                            BorderRadius 5 ]]]
                                            (textToLines errModel.Info.componentStack )
                    ]
                ]

    let errorView (model : Model) (dispatch : Msg -> Unit) =
        match model.ReactErrorModel with
        | None -> div[][str "Oi! What!"]
        | Some errModel ->

            Column.column
                [   Column.Width (Screen.Desktop, Column.Is6)
                    Column.Width (Screen.Tablet, Column.Is6)
                    Column.Width (Screen.Mobile, Column.Is10)
                ] [
                    errorMessage errModel dispatch
                ]



    let view model dispatch =
        Hero.hero
            [ Hero.Color IsPrimary; Hero.IsFullHeight ]
            [ Hero.head []
                [ Navbar.navbar []
                    [ Container.container []
                        [ navBrand model.IsBurgerOpen dispatch
                          navMenu model.CurrentPage model.IsBurgerOpen ] ] ]

              Hero.body []
                [
                    (*
                     *   Container needed to make columns layout full width, apparently
                     *)
                    Container.container [] [
                        Columns.columns [ Columns.IsCentered; Columns.IsVCentered ] [
                            yield LoaderView model.BusyMessage

                            yield
                                match model with
                                | { CurrentPage = Router.Counter;
                                    CounterModel = Some counterModel } ->
                                        YourNamespace.Counter.View.root counterModel (dispatch << CounterMsg)

                                | { CurrentPage = Router.LoadData _;
                                    LoadDataModel = Some dataModel } ->
                                        YourNamespace.LoadData.View.root dataModel (dispatch << LoadDataMsg)

                                | { CurrentPage = Router.About;
                                    AboutModel = Some homeModel } ->
                                        YourNamespace.About.View.root homeModel (dispatch << AboutMsg)

                                | _ -> //{ CurrentPage = Router.Missing(_) } ->
                                    PageNotFound

                                |> YourNamespace.Common.ReactErrorBoundary.renderCatchFn
                                        (fun (error, info) ->
                                            Dom.console.info ("SubComponent failed to render", info, error)
                                            dispatch<<ReactErrorMsg<<ReactError <| (error, info) )
                                        (errorView model dispatch)
                        ]
                    ]
                ]

              StatusBar [
                  div
                    [ Style [ Display DisplayOptions.Flex ] ] [
                        div [ Style [ FlexGrow 1; MarginRight 10 ] ] [
                            NotificationMessage model.Message dispatch ]

                        SafeComponentLinks
                    ]
                ]
            ]

    let view' (model : Model) (dispatch : Msg -> unit) =
        div [ Class "is-main is-primary" ]
            [   Navbar.navbar []
                    [ Container.container []
                        [ navBrand model.IsBurgerOpen dispatch
                          navMenu model.CurrentPage model.IsBurgerOpen ] ]

                Container.container
                    [
                        Container.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ] ]
                    [
                        yield LoaderView model.BusyMessage

                        match model with
                        | { CurrentPage = Router.Counter;
                            CounterModel = Some counterModel } ->
                                yield YourNamespace.Counter.View.root counterModel (dispatch << CounterMsg)

                        | { CurrentPage = Router.LoadData _;
                            LoadDataModel = Some dataModel } ->
                                yield YourNamespace.LoadData.View.root dataModel (dispatch << LoadDataMsg)

                        | { CurrentPage = Router.About;
                            AboutModel = Some homeModel } ->
                                yield YourNamespace.About.View.root homeModel (dispatch << AboutMsg)
                                //yield div [] [str "Here's Home!"]

                        | _ -> //{ CurrentPage = Router.Missing(_) } ->
                                yield PageNotFound
                    ]


                StatusBar [
                  div
                    [ Style [ Display DisplayOptions.Flex ] ]
                      [
                        div [ Style [ FlexGrow 1; MarginRight 10 ] ]
                            [ NotificationMessage model.Message dispatch ]

                        SafeComponentLinks
                    ]
                ]
            ]

