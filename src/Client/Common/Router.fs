namespace YourNamespace.Common


module Router =
    open Browser
    open Elmish.UrlParser
    open Elmish.Navigation
    open Fable.React.Props

    type ErrorPage =
        | NotFound404
        | ServerError500

    type Subsection =
        | Index
        | Show of int

    type Page =
        | About
        | Counter
        | LoadData of Subsection
        | Missing of ErrorPage

    let MainMenuLinks =
        [   Counter, "The Counter"
            LoadData(Index), "A Data Page"
            Missing(ErrorPage.NotFound404), "A Missing Page"
            About, "About" ]

    let private toHashRoute page =
        match page with
        | About -> "#readme"
        | Counter -> "#counter"
        | LoadData page ->
            match page with
            | Index -> "#subsection/index"
            | Show linkId -> sprintf "#subsection/show/%d" linkId
        | Missing _ -> "#error"

    let pageParser: Parser<Page->Page,Page> =
        oneOf [
            map (Subsection.Index |> LoadData) (s "subsection" </> s "index")
            map (Subsection.Show >> LoadData) (s "subsection" </> s "show" </> i32)
            map (NotFound404 |> Missing) (s "this is an obviously dodgy url")
            map (About) (s "readme")
            map Counter (s "counter")
            map Counter top ]

    let modifyLocation page = Dom.window.location.href <- toHashRoute page

    let modifyUrl page = page |> toHashRoute |> Navigation.modifyUrl

    let href = Href<<toHashRoute