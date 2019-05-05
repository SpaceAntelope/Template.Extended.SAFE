namespace YourNamespace.LoadData.Show

module Data =
    open Shared
    open YourNamespace.Common.Data

    let fetchRemoteDataById index =
        fetchAs<RemoteData>
            (sprintf "api/LoadData/%d" index)
            <| decoder()


