namespace Screenshot.Lib

open System.Drawing

module internal Graphics =
    let copyFromScreen (sourceX, sourceY, destX, destY) w h (g: Graphics) =
        g.CopyFromScreen(sourceX, sourceY, destX, destY, Size(w, h))
        ()
    
    let setSize (img: Bitmap) =
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
    