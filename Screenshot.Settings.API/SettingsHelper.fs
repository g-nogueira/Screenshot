namespace Screenshot.Settings.API

open System
open System.IO
open Screenshot.Common.FSharp
open FSharp.Json
open FSharp.Core

module SettingsHelper =
    let settingsFilename = "settings.json"
    let defaultSettingsFilePath = "defaults/settings.json"
    
    [<CLIMutable>]
    type Settings = {
        hotkeySelectArea: string
    }
    
    let getRootSaveFolderLocation () : Path.Path =
        let localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
        let result = Path.combine [|localAppData; "Screenshot"|]
        
        if Path.Exists result |> not then
            Directory.CreateDirectory result |> ignore
        

        
        result
        
    let getGeneralSaveFileLocation () : Path.Path =
        Path.combine [|getRootSaveFolderLocation(); settingsFilename|]
        
            
    let replaceWithDefault () =
        File.Copy(defaultSettingsFilePath,getGeneralSaveFileLocation(), true)
    
        
    let defaultSettings : Settings =
        File.ReadAllText defaultSettingsFilePath
        |> Json.deserialize<Settings>
        
    let tryDeserializeOrDefault def json =
        match json |> String.IsNullOrWhiteSpace with
        | true -> def
        | false -> Json.deserialize<Settings> json
        
    let getOrCreateGeneralSettings () : Settings =
        let location = getGeneralSaveFileLocation ()
        
        File.Exists location
        |> Boolean.onFalse replaceWithDefault
        |> ignore
        
        // TODO: Read file async and Implement AsyncResult
        File.ReadAllText location
        |> tryDeserializeOrDefault defaultSettings 
        
    let updateGeneralSettings (settings: Settings) =
        let location = getGeneralSaveFileLocation ()
        let json = Json.serialize settings
        
        // TODO: Implement AsyncResult
        File.WriteAllText (location, json)
        
        ()
        

