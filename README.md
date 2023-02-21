# ModData 1.2.0

**Pre-Release - Ready for testing**

TLD utility mod for saving/loading custom mod data

**Now uses an internal cache, populated when save is loaded and saves when SaveGameSystem.SaveCompletedInternal completed**

Patches into SaveGameSlots.CreateSlot & GameManager.LoadSaveGameSlot to capture **slotName** and create the .moddata file ([Example File](./example/))

Will only allow saving/loading of data during a loaded game.

* file = \\Mods\\ModData\\**slotName**.moddata (.zip file)
* default entry filename = **ModName**
* suffix entry filename = **ModName**_**Suffix**

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

### Todo

Open to suggestions :)