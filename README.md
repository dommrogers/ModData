# ModData 1.2.0

**Pre-Release - Ready for testing**

TLD utility mod for saving/loading custom mod data

* Simple usage
* Detects the save name when loaded/created
* Only allows load/save while a save game is active
* Uses an internal cache to reduce disk read/writes
* Deletes associated .moddata file when the save is deleted
* [Example File](./example/)

default file = \\Mods\\ModData\\**slotName**.moddata (.zip file)
default entry filename = **modName**
suffix entry filename = **modName**_**Suffix**

## Usage

```cs
using ModData;
```
```cs
ModDataManager dataManager = new ModDataManager(string modName, [bool debug = false]);
```
```cs
bool dataManager.Save(string data) 
bool dataManager.Save(string data, string? suffix)
```
```cs
string? dataManager.Load() 
string? dataManager.Load(string? suffix)
```

## Installation

1. If you haven't done so already, install MelonLoader by downloading and running [MelonLoader.Installer.exe](https://github.com/HerpDerpinstine/MelonLoader/releases/latest/download/MelonLoader.Installer.exe)
2. Download the latest version of `ModData.dll` from the [releases page](https://github.com/dommrogers/ModData/releases)
3. Move `ModData.dll` into the Mods folder in your TLD install directory

## Thanks

Thank you to all the helpful devs in the [TLD Modding Discord](https://discord.gg/EhBWKRx) for their patience and help