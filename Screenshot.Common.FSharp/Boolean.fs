namespace Screenshot.Common.FSharp

module Boolean =
    let inline onTrue action =
        function
        | false -> false
        | true -> action
