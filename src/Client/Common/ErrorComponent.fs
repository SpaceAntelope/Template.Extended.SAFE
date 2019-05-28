namespace YourNamespace.Common
open Fulma
open Fable.React
open Fable.React.Props
open ReactErrorBoundary
open YourNamespace.Common.View
open Fable.FontAwesome
open Fable.Core
open Elmish

module ErrorComponent =

    //type Model = ReactErrorBoundary.ErrorBoundaryProps

    // type Msg =
    //     | Reload
    //     | IsExpanded of bool

    type Model = {
        Exn: System.Exception
        Info: InfoComponentObject
        IsExpanded: bool
    }

    [<Emit("location.reload()")>]
    let jsLocationReload() = Util.jsNative

    let textToLines (text:string) =
        text.Split('\n')
        |> List.ofArray
        |> List.map (fun line -> div [] [str line])

    let update msg model = //(msg : Common.Msg) (model : Model) : Model * Cmd<Msg> =
        Browser.Dom.console.info("[ERROR COMPONENT]", sprintf "%A" msg)
        model, Cmd.ofMsg (Types.ErrorComponentMsg msg)


    (*
     * To appear when error model is none, meaning that reloading
     * failed to fix the rendering issue and a page reload is necessary.
     * It is model agnostic and dispatches no messages.
     *)
    let altView model (dispatch: Types.Msg -> unit) =
        Box.box' [
            Props [AddAnimation "fadeIn"]
            Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Justified) ]] [
            Heading.h4 [Heading.Props [ Style [Color "tomato"]]][str "Oops..."]
            p[][
            //hr[]
            str "If you are seeing this, it means that while the page did reset,
                 either the rendering inconsistency persists or the Error Boundary's
                 state.hasError could not be set to 'false'."]

            Container.container [ Container.IsFluid ] [
                Content.content [ Content.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered)] ] [
                    Button.button
                        [   Button.Color IsPrimary
                            Button.IsOutlined
                            Button.OnClick (fun e ->jsLocationReload()) ] [
                            str "Reload"
                            Fa.i [
                                Fa.Props[Style[MarginLeft 5]]
                                Fa.Solid.Sync ][]
                ] ] ]
            br []
            div [ Style [
                    TextAlign TextAlignOptions.Right
                    FontSize "small"]][

                strong [Style[Color "#BCBBBB"]][str "See also: "]

                a [ Href "https://stackoverflow.com/questions/48121750/browser-navigation-broken-by-use-of-react-error-boundaries"

                    Style [
                        Color "#F48024"
                        FontStyle "italic"
                        TextDecoration "underlined"]] [
                    str "Browser navigation broken by use of React Error Boundaries"

                    Fa.i [
                        Fa.Brand.StackOverflow
                        Fa.Size Fa.FaLarge
                        Fa.Props [ Style [ MarginLeft 5 ]]] []
            ]   ]   ]

    let view model (dispatch: Types.Msg -> unit) =
        let reloadMsg =
            Types.ReactErrorMsg.Reload
            |> Types.Msg.ErrorComponentMsg

        let expandMsg =
            Types.ReactErrorMsg.IsExpanded
            >> Types.Msg.ErrorComponentMsg

        Message.message [
            Message.Color IsDanger
            Message.Props [
                AddAnimation "bounceIn"]][
                    Message.header [] [
                        str "React rendering appears to have imploded"
                        Delete.delete [ Delete.OnClick (fun e -> dispatch reloadMsg)] [] ]

                    Message.body [][
                        yield Text.p [
                                Modifiers [
                                    Modifier.TextSize (Screen.All, TextSize.Is4)
                                    Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ]] [
                                    str <| model.Exn.Message.Replace(model.Info.componentStack,"") ]

                        yield Text.p [
                            Props[OnClick (fun e -> dispatch <| expandMsg (not model.IsExpanded))]] [
                                a [] [
                                    span [Style[Margin 5]][str "Details"]
                                    Fa.i [
                                        yield if not model.IsExpanded
                                            then Fa.Solid.AngleDown
                                            else Fa.Solid.AngleLeft][] ] ]

                        if model.IsExpanded
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
                                            (textToLines model.Info.componentStack )
                    ]
                ]
