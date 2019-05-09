namespace YourNamespace

module Client =

    open YourNamespace.Common
    open Elmish
    open Elmish.React
    open Elmish.UrlParser
    open Elmish.HMR

    #if DEBUG
    open Elmish.Debug
    #endif


    Program.mkProgram Root.State.init Root.State.update Root.View.view
    |> Program.toNavigable (parseHash Router.pageParser) Root.State.urlUpdate
    #if DEBUG
    |> Program.withConsoleTrace
    // |> Program.withHMR -- Don't worry, you still get HMR
    #endif
    |> Program.withReactBatched "elmish-app"
    #if DEBUG
    |> Program.withDebugger
    #endif
    |> Program.run
