namespace YourNamespace.LoadData.Show

module Types =
    open Elmish
    open Shared
    open YourNamespace.Common.Types

    type Model = {
        Data: RemoteData option
    }

    type Msg =
    | LoadData of index:int
    | DataLoaded of Result<RemoteData, string>
    | GlobalMsg of YourNamespace.Common.Types.Msg

    let NotifyMsg =
        YourNamespace.Common.Types.Notify
        >> GlobalMsg

    let NotifyErrorMsg =
        NotificationText.Danger
        >> NotifyMsg

    let NotifyWarningMsg =
        NotificationText.Warning
        >> NotifyMsg