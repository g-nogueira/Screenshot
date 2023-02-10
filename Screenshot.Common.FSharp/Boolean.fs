namespace Screenshot.Common.FSharp

module Boolean =
    let inline onTrue action =
        function
        | false -> None
        | true -> Some (action())
        
    let inline onFalse action =
        function
        | true -> None
        | false -> Some (action())
