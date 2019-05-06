namespace YourNamespace.Common


module Router =
    open Fable.Import
    open Elmish.UrlParser
    open Elmish.Navigation
    open Browser
    open Fable.React.Props

    type Subsection =
        | Index
        | Show of int

    type Page =
        | Home
        | Counter
        | LoadData of Subsection
        | Missing

    let private toHashRoute page =
        match page with
        | Home -> "#"
        | Counter -> "#counter"
        | LoadData page ->
            match page with
            | Index -> "#subsection/index"
            | Show linkId -> sprintf "#subsection/show/%d" linkId

    let pageParser: Parser<Page->Page,Page> =
        oneOf [
            map (Subsection.Index |> LoadData) (s "subsection" </> s "index")
            map (Subsection.Show >>  LoadData) (s "subsection" </> s "show" </> i32)
            map Counter (s "counter")
            map Home top ]

    let modifyLocation page = Dom.window.location.href <- toHashRoute page

    let modifyUrl page = page |> toHashRoute |> Navigation.modifyUrl

    //
    let href = Href<<toHashRoute