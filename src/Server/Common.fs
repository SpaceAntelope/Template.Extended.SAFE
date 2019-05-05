namespace YourNamespace

open Dapper
open System.Data.Common
open FSharp.Control.Tasks
open System.Collections.Generic

module Config =
    type Config = { ConnectionString: string option }

module Database =
    let inline (=>) key value = key, box value

