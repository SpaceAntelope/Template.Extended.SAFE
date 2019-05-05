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
    | UnexpectedError of exn