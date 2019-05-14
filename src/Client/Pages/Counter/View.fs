namespace YourNamespace.Counter

module View =
    open Fable.React
    open Fable.React.Props
    open Fulma
    open YourNamespace.Counter.Types
    open Fable.FontAwesome

    let show = function
    | { Counter = Some counter } -> string counter.Value
    | { Counter = None } -> "Loading..."

    let error _ _ = div [] [ str "Rendering error" ]

    let CounterBox (model : Model) (dispatch : Msg -> unit) =
        if model.Counter.Value.Value = 55
        then
            dispatch Increment
            failwith "Oh no! Some terrible bug must have occured."

        Box.box'
            [ ]
            [ Field.div [ Field.IsGrouped ]
                [ Control.p [ Control.IsExpanded ]
                    [ Input.text
                        [ Input.Disabled true
                          Input.Value(show model) ] ]
                  Control.p []
                    [ Button.a
                        [ Button.Color IsPrimary
                          Button.OnClick(fun _ -> dispatch Increment) ]
                        [ Fa.i [ Fa.Solid.Plus ] [] ] ]
                  Control.p []
                    [ Button.a
                        [ Button.Color IsPrimary
                          Button.OnClick(fun _ -> dispatch Decrement) ]
                        [ Fa.i [ Fa.Solid.Minus ] [] ] ]
                  Control.p []
                    [ Button.a
                        [ Button.Color IsDanger ]
                        [ Fa.i [ Fa.Solid.Bomb ] [] ] ]
                    ] ]
                    |> YourNamespace.Common.ReactErrorBoundary.renderCatchSimple(error model dispatch)

    let root model dispatch =
        Column.column
            [ Column.Width(Screen.All, Column.IsHalf)
              Column.Props [ YourNamespace.Common.View.AddAnimation "fadeIn" ]
            ]
            [ Heading.p
                [] [ str "SAFE Template" ]
              CounterBox model dispatch ]
        //|> YourNamespace.Common.ReactErrorBoundary.renderCatchSimple
         |> YourNamespace.Common.ReactErrorBoundary.renderCatchFn
             (fun (error, info) ->
                Browser.Dom.console.log ("Failed to render" + info.componentStack, error))
                (error model dispatch)
