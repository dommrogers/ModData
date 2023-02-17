# ModData 1.1.0

**WIP - NOT FOR PRODUCTION USE**

TLD utility mod for saving/loading custom mod data

Patches into GameManager.LoadSaveGameSlot to capture **slotName** and create the .moddata file ([Example File](./example/))

Will only allow saving/loading of data during a loaded game.

* file = \\Mods\\ModData\\**slotName**.moddata (.zip file)
* default entry filename = **ModName**
* suffix entry filename = **ModName**_**Suffix**

## Usage

```cs
using ModData;
```

```cs
ModDataManager dataManager = new ModDataManager("ModName");
```
```cs
void dataManager.Save(string data) 
void dataManager.Save(string data, string? suffix)
```
```cs
string? dataManager.Load() 
string? dataManager.Load(string? suffix)
```

### Todo
* additional validation
* return bool for Save()
* file & entry name sanitation
* remove ModData file when save game is deleted ?
