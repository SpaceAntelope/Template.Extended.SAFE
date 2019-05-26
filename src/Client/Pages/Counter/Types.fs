namespace YourNamespace.Counter

module Types =
    open Shared

    type Model =
        { Counter : Counter option }

    type Msg =
        | Increment
        | Decrement
        | Goto of int
        | InitialCountLoaded of Result<Counter, string>
        | GlobalMsg of YourNamespace.Common.Types.Msg
