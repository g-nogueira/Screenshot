namespace Screenshot.Settings.API

#r "FSharp.Json.dll"
open System
open System.IO
open Screenshot.Common.FSharp
open FSharp.Json

module SettingsHelper =
    let settingsFilename = "\\settings.json"
    
    type Settings = {
        hotkeySelectArea: string
    }
    
    let getRootSaveFolderLocation () : Path.Path =
        let localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
        let result = Path.Combine [|localAppData; "Screenshot"|]
        
        if Path.Exists result |> not then
            Directory.CreateDirectory result |> ignore
        
        result
        
    let getGeneralSaveFileLocation () : Path.Path =
        Path.Combine [|getRootSaveFolderLocation(); settingsFilename|]
        
    let getGeneralSettings () : Settings =
        let location = getGeneralSaveFileLocation ()
        
        // TODO: Read file async and Implement AsyncResult
        let json = File.ReadAllText location
        
        Json.deserialize<Settings> json
        
    let setGeneralSettings (settings: Settings) =
        let location = getGeneralSaveFileLocation ()
        let json = Json.serialize settings
        
        // TODO: Implement AsyncResult
        File.WriteAllText (location, json)
        
        ()