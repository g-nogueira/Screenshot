namespace Screenshot.Lib

open System.Drawing
open Screenshot.Lib.Graphics        

module Screenshot =
    open Graphics
    
    /// Captures a screenshot
    /// Based on https://www.codeproject.com/Tips/817001/Saving-a-screenshot-using-Csharp-A-K-A-Console-Mon
    let setSize (w: int, h: int) =
        bitmap w h
    
    let selectArea (sourceX, sourceY) (bitmap: Bitmap) =
        bitmap
        |> screenshot (sourceX,sourceY)
        
    let save (path: string) (img: Bitmap) =
        img.Save path
        
    let captureScreen (path:string) (w: int) (h:int) =
        bitmap w h
        |> screenshot (0,0)
        |> save path
