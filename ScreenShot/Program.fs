// For more information see https://aka.ms/fsharp-console-apps
open System.Drawing
open Screenshot.Lib
open Screenshot.Lib.Screenshot
        
let capture (path:string) (w: int) (h:int) =
    captureScreen path w h
    

[<EntryPoint>]
let main args =
    printfn "Starting..."
    capture "C:\\Users\\gnogueira\\Desktop\\screenshot1.jpeg" 1000 900
    
    0
    