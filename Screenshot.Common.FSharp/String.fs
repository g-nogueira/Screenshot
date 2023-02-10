namespace Screenshot.Common.FSharp

module String =
    let inline onNullOrWhiteSpace action str =
        match System.String.IsNullOrWhiteSpace str with
        | true -> Some (action())
        | false -> None
