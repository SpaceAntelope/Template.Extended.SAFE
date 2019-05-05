namespace YourNamespace.Counter

module Data =
    open Shared
    open YourNamespace.Common.Data

    let InitializeCounter =
        fetchAs<Counter> "/api/init" <| decoder()