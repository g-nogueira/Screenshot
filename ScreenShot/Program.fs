// For more information see https://aka.ms/fsharp-console-apps
open System.Drawing

module Graphics =
    let copyFromScreen (sourceX, sourceY, destX, destY) w h (g: Graphics) =
        g.CopyFromScreen(sourceX, sourceY, destX, destY, Size(w, h))
        ()
    
    let size (img: Bitmap) =
        Size(img.Width, img.Height)
        
    let save (path: string) (img: Bitmap) =
        img.Save path
        
    let bitmap (w: int) (h: int) =
        new Bitmap(w,h)
        
    let screenshot (sourceX, sourceY) (img: Bitmap) =
        img
        |> Graphics.FromImage
        |> copyFromScreen (sourceX, sourceY, 0, 0) img.Width img.Height
        
        img
        

module ScreenShot =
    open Graphics
    /// Captures a screenshot
    /// Based on https://www.codeproject.com/Tips/817001/Saving-a-screenshot-using-Csharp-A-K-A-Console-Mon
    let capture (path:string) (w: int) (h:int) =
        bitmap w h
        |> screenshot (0,0)
        |> save path

[<EntryPoint>]
let main args =
    printfn "Starting..."
    ScreenShot.capture "C:\\Users\\gnogueira\\Desktop\\screenshot1.jpeg" 1000 900
    
    0
    