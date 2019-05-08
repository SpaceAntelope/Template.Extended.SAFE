namespace YourNamespace.Root

module Types =
    open YourNamespace.Common.Types
    open YourNamespace.Common.Router

    type Model = {
          CounterModel: YourNamespace.Counter.Types.Model option
          LoadDataModel: YourNamespace.LoadData.Types.Model option
          AboutModel: YourNamespace.About.Types.Model option
          Message: NotificationText
          BusyMessage: string option
          CurrentPage: Page
          IsBurgerOpen: bool
    }

    type Msg =
    | GlobalMsg of YourNamespace.Common.Types.Msg
    | AboutMsg of YourNamespace.About.Types.Msg
    | CounterMsg of YourNamespace.Counter.Types.Msg
    | LoadDataMsg of YourNamespace.LoadData.Types.Msg
    | ToggleBurger of state:bool
    | ClearNotification

