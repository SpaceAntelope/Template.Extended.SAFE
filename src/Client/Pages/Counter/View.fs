namespace YourNamespace.Counter
open Fable.React

module View =
    open Fulma
    open YourNamespace.Counter.Types

    let show = function
    | { Counter = Some counter } -> string counter.Value
    | { Counter = None   } -> "Loading..."

    let CounterBox (model : Model) (dispatch : Msg -> unit) =
        Box.box' [ ]
            [ Field.div [ Field.IsGrouped ]
                [ Control.p [ Control.IsExpanded ]
                    [ Input.text
                        [ Input.Disabled true
                          Input.Value (show model) ] ]
                  Control.p [ ]
                    [ Button.a
                        [ Button.Color IsPrimary
                          Button.OnClick (fun _ -> dispatch Increment) ]
                        [ str "+" ] ]
                  Control.p [ ]
                    [ Button.a
                        [ Button.Color IsPrimary
                          Button.OnClick (fun _ -> dispatch Decrement) ]
                        [ str "-" ] ] ] ]

    let view model dispatch =
        Column.column
            [ Column.Width (Screen.All, Column.Is6)
              Column.Offset (Screen.All, Column.Is3) ]
            [ Heading.p
                [ ] [ str "SAFE Template" ]
              CounterBox model dispatch ]