namespace YourNamespace.LoadData.Index

module Types =
    open Elmish
    open Shared
    open YourNamespace.Common.Types

    type IdxStr = int*string

    type Model = {
        Data: IdxStr list option
    }

    type Msg =
    | LoadData
    | DataLoaded of Result<IdxStr list, string>
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