namespace Screenshot.Common.FSharp

open System.IO

module Path =
    type Path = string
    
    let Combine (paths:string[]) : Path =
        Path.Combine paths


