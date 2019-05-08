namespace YourNamespace.Common

// based on https://github.com/fable-compiler/fable-react/blob/master/docs/react-error-boundaries.md
module ReactErrorBoundary =
    open Fable.React

    type [<AllowNullLiteral>] InfoComponentObject =
        abstract componentStack: string with get

    //[<Pojo>]
    type ErrorBoundaryProps =
        { Inner : ReactElement
          ErrorComponent : ReactElement
          OnError : exn * InfoComponentObject -> unit }

    //[<Pojo>]
    
    type ErrorBoundaryState =
        { HasErrors : bool }

    // See https://github.com/MangelMaxime/Fulma/blob/master/docs/src/Widgets/Showcase.fs
    // See https://reactjs.org/docs/error-boundaries.html
    type ErrorBoundary(props) =
        inherit Component<ErrorBoundaryProps, ErrorBoundaryState>(props)
        do base.setInitState({ HasErrors = false })

        override x.componentDidCatch(error, info) =
            let info = info :?> InfoComponentObject
            x.props.OnError(error, info)
            x.setState({ HasErrors = true })

        override x.render() =
            if (x.state.HasErrors) then
                x.props.ErrorComponent
            else
                x.props.Inner

    let renderCatchSimple errorElement element =
        ofType<ErrorBoundary,_,_> { Inner = element; ErrorComponent = errorElement; OnError = fun _ -> () } [ ]

    let renderCatchFn onError errorElement element =
        ofType<ErrorBoundary,_,_> { Inner = element; ErrorComponent = errorElement; OnError = onError } [ ]