// namespace YourNamespace

// open System
// open FSharp.Data.Dapper
// open System.Data.SqlClient

// module Repository =
//     OptionHandler.RegisterTypes()

//     let inline (=>) key value = key, box value
//     let private connectionString = System.IO.File.ReadAllText("connection.info")
//     let private connectionF() =
//         Connection.SqlServerConnection(new SqlConnection(connectionString))
//     let querySeqAsync<'R> = querySeqAsync<'R> (connectionF)
//     let querySingleAsync<'R> = querySingleAsync<'R> (connectionF)
//     let querySingleOptionAsync<'R> = querySingleOptionAsync<'R> (connectionF)

//     type RemoteLink =
//         { Id : int64
//           FeedCode : string
//           Title : string
//           Summary : string
//           SummaryHtml : string
//           Url : string
//           Categories : string
//           PublishedDate : DateTime }

//     module RemoteLink =
//         let latest (count : int) =
//             querySeqAsync<RemoteLink> {
//                 parameters (dict [ "Rows" => count ])
//                 script """
// SELECT TOP(@Rows) Id,
//        FeedCode,
//        Title,
//        Summary,
//        SummaryHtml,
//        Url,
//        Categories,
//        PublishedDate
// FROM UNA_RemoteLink
// ORDER BY PublishedDate DESC"""
//             }
