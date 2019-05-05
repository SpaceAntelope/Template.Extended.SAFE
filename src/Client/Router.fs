namespace YourNamespace

open Fable.Core
open Fable.Import
open Fable.React
open Fable.React.Props
open Elmish.UrlParser
open Elmish.Navigation
open Fulma
open Browser


module Router =
    type RemoteLinksPage =
        | Index
        | Show of int

    type Page =
        | Home
        | TestPage
        | RemoteLinks of RemoteLinksPage
        | RemoteSources

    let private toRoute page =
        match page with
        | TestPage -> "#test"
        | RemoteLinks page ->
            match page with
            | Index -> "#links/index"
            | Show linkId -> sprintf "#links/%i" linkId
        | RemoteSources -> "#sources/index"
        | Home -> "#/"

    let pageParser: Parser<Page->Page,Page> =
        oneOf [
            map (RemoteLinksPage.Index |> RemoteLinks) (s "links" </> s "index")
            map (RemoteLinksPage.Show >> RemoteLinks) (s "links" </> i32)
            map RemoteSources (s "sources" </> s "index" )
            map TestPage (s "test")
            map Home top ]

    let modifyLocation page = Dom.window.location.href <- toRoute page

    let modifyUrl page = page |> toRoute |> Navigation.modifyUrl

    let navbarItems (pages : (Page * string) list) =
        [ for page, title in pages do
              yield  Navbar.Item.a
                          [ Navbar.Item.Props
                                [ OnClick(fun _ -> modifyLocation page) ] ]
                          [ ofString title ] ]
    type Model = {
        CurrentPage: Page
        IsMenuCollapsed: bool
    }

    let urlUpdate (result: Option<Page>) model =
        match result with
        | None ->
            Dom.console.error("Error parsing url: " + window.location.href)
            JS.console.error("Error parsing url: " + window.location.href)

            model, modifyUrl model.CurrentPage

        | Some page ->
            let model = { model with CurrentPage = page }

            match page with
            | Router.Question questionPage ->
                let (subModel, subCmd) = Question.Dispatcher.State.init questionPage
                { model with QuestionDispatcher = Some subModel }, Cmd.map QuestionDispatcherMsg subCmd
            | Router.Home ->
                let (subModel, subCmd) = Question.Dispatcher.State.init Router.QuestionPage.Index
                { model with QuestionDispatcher = Some subModel }, Cmd.map QuestionDispatcherMsg subCmd


    // let modifyUrl route =
    //     route |> toHash |> Navigation.modifyUrl
    // let urlUpdate (result: Option<Page>) model =
    //     match result with
    //     | None ->
    //         JS.console.error("Error parsing url: " + window.location.href)
    //         model, Router.modifyUrl model.CurrentPage
    //     | Some page ->
    //         let model = { model with CurrentPage = page }
    //         match page with
    //         | Router.Question questionPage ->
    //             let (subModel, subCmd) = Question.Dispatcher.State.init questionPage
    //             { model with QuestionDispatcher = Some subModel }, Cmd.map QuestionDispatcherMsg subCmd
    //         | Router.Home ->
    //             let (subModel, subCmd) = Question.Dispatcher.State.init Router.QuestionPage.Index
    //             { model with QuestionDispatcher = Some subModel }, Cmd.map QuestionDispatcherMsg subCmd
