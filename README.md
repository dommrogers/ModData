# ModData

**WIP - TLD utility mod for saving/loading custom mod data.**

**TODO - Validation/Exceptions**

* folder = \\Mods\\ModData\\\_saveSlotName_\\\_modName_
* file ext = .moddata
* default filename = Global
* file format = plain text (default) or base64encoded

**using ModData;**

## Usage

returns bool
```cs
ModDataManager.Save(string saveSlotName, string modName, string data);
ModDataManager.Save(string saveSlotName, string modName, string data, bool useEncoding = false)
ModDataManager.Save(string saveSlotName, string modName, string data, string? filename = null)
ModDataManager.Save(string saveSlotName, string modName, string data, string? filename = null, bool useEncoding = false)
```

returns string
```cs
ModDataManager.Load(string saveSlotName, string modName)
ModDataManager.Load(string saveSlotName, string modName, string? filename = null)
```