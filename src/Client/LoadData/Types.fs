namespace YourNamespace.LoadData

module Types =
    open Elmish
    open Shared
    open YourNamespace.Common.Types

    type Model = {
        IndexModel:  Index.Types.Model option
        ShowModel: Show.Types.Model option
    }

    type Msg =
    | IndexMsg of Index.Types.Msg
    | ShowMsg of Show.Types.Msg
    | GlobalMsg of YourNamespace.Common.Types.Msg


    // let NotifyMsg =
    //     YourNamespace.Common.Types.Notify
    //     >> GlobalMsg

    // let NotifyErrorMsg =
    //     NotificationText.Danger
    //     >> NotifyMsg

    // let NotifyWarningMsg =
    //     NotificationText.Warning
    //     >> NotifyMsg