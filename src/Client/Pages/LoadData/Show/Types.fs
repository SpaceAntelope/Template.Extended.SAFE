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
    | UnexpectedError of exn
