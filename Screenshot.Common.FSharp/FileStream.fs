namespace Screenshot.Common.FSharp

open System.IO

module FileStream =
    let close (fs: FileStream) =
        fs.Close()
        fs