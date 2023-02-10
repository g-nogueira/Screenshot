namespace Screenshot.Common.FSharp

open System.IO

module Path =
    type Path = string
    
    let combine (paths:string[]) : Path =
        Path.Combine paths


