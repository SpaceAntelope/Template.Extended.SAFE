namespace YourNamespace.Common
open Fulma
open Fable.React
open Fable.React.Props

module ErrorComponent = 

    type Model = ReactErrorBoundary.ErrorBoundaryProps

    type Msg = 
        | Reload
    

    let update msg model =
        ()

    // let view (model: Model) dispatch = 
    //     div
    //         [ ClassName "animated bounceIn" ]
    //         [
    //             Notification.notification
    //                 [   Notification.Color IsDanger; ]
    //                 [
    //                     Heading.h1
    //                         [ Heading.Modifiers [Modifier.TextAlignment (Screen.All, TextAlignment.Left)]]
    //                         [ str "React rendering error" ]
    //                     hr []
    //                     table [][
    //                         tr [][
    //                             td [Style [FontWeight "bold"; TextAlign TextAlignOptions.Right]] [str "Component"]
    //                             td [] [str <| model.ErrorComponent.GetType().FullName ] ]
    //                         tr [][
    //                             td [Style [FontWeight "bold"; TextAlign TextAlignOptions.Right]] [str "Message"]
    //                             td [] [str <| model.Inner ] ]
    //                     ]
    //                     str model.Inner
    //                     // Heading.h4
    //                     //     [ Heading.Modifiers [Modifier.TextAlignment (Screen.All, TextAlignment.Right)]]
    //                     //     [ str "Page not found"]
    //                 ]
    //         ]
