namespace Shared

open System

type Counter =
    { Value : int }

type RemoteData =
    { Id: int
      Name: string
      EmbedReference: string
      ClassName: string
      Theme: string }
