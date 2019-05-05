namespace YourNamespace.LoadData.Index

module Data =
    open YourNamespace.Common.Data
    open Types

    let fetchRemoteDataIndex = fetchAs<IdxStr list> "api/LoadData" <| decoder()
