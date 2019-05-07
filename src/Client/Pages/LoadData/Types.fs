namespace YourNamespace.LoadData

module Types =

    type Model = {
        IndexModel: Index.Types.Model option
        ShowModel: Show.Types.Model option
    }

    type Msg =
    | IndexMsg of Index.Types.Msg
    | ShowMsg of Show.Types.Msg