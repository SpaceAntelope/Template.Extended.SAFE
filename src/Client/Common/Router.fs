namespace YourNamespace.Common


module Router =
    open Fable.Import
    open Elmish.UrlParser
    open Elmish.Navigation
    open Browser
    open Fable.React.Props

    type ErrorPage =
        | NotFound404
        | ServerError500

    type Subsection =
        | Index
        | Show of int

    type Page =
        | Home
        | Counter
        | LoadData of Subsection
        | Missing of ErrorPage

    let MainMenuLinks =
        [   Home, "Home"
            Counter, "The Counter"
            LoadData(Index), "A Data Page"
            Missing(ErrorPage.NotFound404), "A Missing Page" ]

    let private toHashRoute page =
        // Note Missing page is not matched, i.e. it's not meant to be accessible via url
        match page with
        | Home -> "#"
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
            map Counter (s "counter")            
            map Home top ]

    let modifyLocation page = Dom.window.location.href <- toHashRoute page

    let modifyUrl page = page |> toHashRoute |> Navigation.modifyUrl

    //
    let href = Href<<toHashRoute