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

    let CounterBox (model : Model) (dispatch : Msg -> unit) =
        Box.box' [ ] [
            Field.div [ Field.IsGrouped ] [
                Control.p [ Control.IsExpanded ] [
                    Input.text [

                        Input.Disabled true
                        Input.Value(show model) ] ]
                Control.p [] [
                    Button.a
                        [   Button.Color IsPrimary
                            Button.OnClick(fun _ -> dispatch Increment) ]
                        [ Fa.i [ Fa.Solid.Plus ] [] ] ]
                Control.p []
                    [ Button.a
                        [ Button.Color IsPrimary
                          Button.OnClick(fun _ -> dispatch Decrement) ]
                        [ Fa.i [ Fa.Solid.Minus ] [] ] ]
                Control.p []
                    [   yield Button.a
                            [ Button.Color IsDanger
                              Button.OnClick (fun e -> dispatch <| Goto(45) )  ]
                            [ Fa.i [ Fa.Solid.Bomb ] [] ]

                        if model.Counter.Value.Value = 45
                        then
                            (* An Option type can't be rendered, which mekes this
                             * convenient way to cause an arbitrary rendering crash
                             * to test error boundary components.
                             *)
                            yield unbox <| Button.OnClick(ignore)  ]
                    ] ]

    let root model dispatch =
        Column.column
            [ Column.Width(Screen.All, Column.IsHalf)
              Column.Props [ YourNamespace.Common.View.AddAnimation "fadeIn" ]
            ] [
                Heading.p [] [
                    str "SAFE Template" ]
                CounterBox model dispatch ]