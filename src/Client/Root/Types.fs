namespace YourNamespace.Root

module Types =
    open YourNamespace.Common.Types
    open YourNamespace.Common.Router

    type Model = {
          CounterModel: YourNamespace.Counter.Types.Model option
          LoadDataModel: YourNamespace.LoadData.Types.Model option
          HomeModel: YourNamespace.Home.Types.Model option
          Message: NotificationText
          BusyMessage: string option
          CurrentPage: Page
          IsBurgerOpen: bool
    }

    type Msg =
    | GlobalMsg of YourNamespace.Common.Types.Msg
    | HomeMsg of YourNamespace.Home.Types.Msg
    | CounterMsg of YourNamespace.Counter.Types.Msg
    | LoadDataMsg of YourNamespace.LoadData.Types.Msg
    | ToggleBurger of state:bool
    | ClearNotification

