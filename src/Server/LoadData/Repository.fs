namespace YourNamespace.LoadData

open Shared
open YourNamespace.Database
open System.Data.SqlClient
open FSharp.Control.Tasks
open Dapper

// Database access logic goes here
module Repository =
    let data =
        [ { Id = 1
            Name = "@AresLazarus"
            EmbedReference =
                "https://twitter.com/JasonPandiras?ref_src=twsrc%5Etfw"
            Theme = "dark"
            ClassName = "twitter-timeline" }
          { Id = 2
            Name = "@FableCompiler"
            EmbedReference =
                "https://twitter.com/FableCompiler?ref_src=twsrc%5Etfw"
            Theme = "dark"
            ClassName = "twitter-timeline" }
          { Id = 3
            Name = "@FSharp.org"
            EmbedReference = "https://twitter.com/fsharporg?ref_src=twsrc%5Etfw"
            Theme = ""
            ClassName = "twitter-timeline" }
          { Id = 4
            Name = "@FAKE - F# Make"
            EmbedReference =
                "https://twitter.com/fsharpMake?ref_src=twsrc%5Etfw"
            Theme = ""
            ClassName = "twitter-timeline" } ]

    let FetchAll connStr =
        task { return data |> List.map (fun item -> item.Id, item.Name) }
    let FetchById(itemId : int) =
        task { return data |> List.find (fun item -> itemId = item.Id) }
