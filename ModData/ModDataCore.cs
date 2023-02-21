
namespace ModData;

internal class ModDataCore
{
	internal static string ModDataEntryName { get; } = "ModData.txt";
	internal static string modDataFolderName { get; } = "ModData";
	internal static string modDataFileExt { get; } = ".moddata";
	internal static string modDataRoot { get; private set; } = "";

	internal static string? modDataSaveSlotName { get; private set; } = null;
	internal static string? modDataSaveSlotFile { get; private set; } = null;

	internal static Dictionary<string, string> dataCache = new();

	internal static void InitModDataCore(string saveName)
	{
		InitModDataRoot();
		DebugMsg("Loading slot " + saveName);
		modDataSaveSlotName = saveName;
		InitModDataSaveSlot();

		LoadCache();
	}

	internal static void InitModDataRoot()
	{
		modDataRoot = Path.Combine(MelonEnvironment.ModsDirectory, modDataFolderName);
		if (!Directory.Exists(modDataRoot))
		{
			DebugMsg("Creating modDataRoot (" + modDataRoot + ")");
			Directory.CreateDirectory(modDataRoot);
		}
	}

	internal static void InitModDataSaveSlot()
	{
		if (modDataSaveSlotName == null)
		{
			DebugMsg("No SaveSlot loaded.");
			return;
		}
		modDataSaveSlotFile = Path.Combine(modDataRoot, modDataSaveSlotName + modDataFileExt);

		// create and/or open the saveSlotFile
		if (!File.Exists(modDataSaveSlotFile))
		{
			DebugMsg("Creating modDataSaveSlotFile (" + modDataSaveSlotFile + ")");
			ZipUtils.CreateEmptyFile(modDataSaveSlotFile);
		}
	}

	internal static void DeleteModDataSaveSlot(string slotName)
	{
		InitModDataRoot();

		modDataSaveSlotFile = Path.Combine(modDataRoot, slotName + modDataFileExt);

		if (File.Exists(modDataSaveSlotFile))
		{
			DebugMsg("Deleting modDataSaveSlotFile (" + modDataSaveSlotFile + ")");
			File.Delete(modDataSaveSlotFile);
		}

	}

	internal static void CloseModDataSaveSlot()
	{
		if (modDataSaveSlotName != null)
		{
			modDataSaveSlotName = null;
			modDataSaveSlotFile = null;
			dataCache.Clear();
			DebugMsg("CloseModDataSaveSlot");
		}
	}


	internal static bool WriteEntry(string entryName, string entryData, string? entrySuffix = null)
	{
		if (entrySuffix != null)
		{
			entryName += "_" + entrySuffix;
		}
		if (modDataSaveSlotFile == null)
		{
			DebugMsg("No SaveSlot loaded.");
			return false;
		}

		if (dataCache.ContainsKey(entryName))
		{
			dataCache.Remove(entryName);
		}

		dataCache.Add(entryName, entryData);
		return true;
	}

	internal static string? ReadEntry(string entryName, string? entrySuffix = null)
	{
		if (entrySuffix != null)
		{
			entryName += "_" + entrySuffix;
		}
		if (modDataSaveSlotFile == null)
		{
			DebugMsg("No SaveSlot loaded.");
			return null;
		}

		if (dataCache.ContainsKey(entryName))
		{
			return dataCache[entryName];
		}

		return null;
	}

	internal static void LoadCache()
	{
		if (modDataSaveSlotFile == null)
		{
			MelonLogger.Msg(ConsoleColor.Cyan, "No SaveSlot loaded.");
			return;
		}
		List<string> entries = ZipUtils.GetEntries(modDataSaveSlotFile);
		if (entries != null && entries.Count > 0)
		{
			foreach (string entry in entries)
			{
				string? data = ZipUtils.ReadEntry(modDataSaveSlotFile, entry);
				if (data != null)
				{
					WriteEntry(entry, data);
				}
			}
		}
		DebugMsg("Cache Loaded");
	}

	internal static void SaveCache()
	{
		if (modDataSaveSlotFile == null)
		{
			DebugMsg("No SaveSlot loaded.");
			return;
		}
		if (dataCache != null && dataCache.Count > 0)
		{
			foreach (KeyValuePair<string,string> entry in dataCache)
			{
				if (entry.Key != null && entry.Value != null)
				{
					string entryString = entry.Value;
					if (entry.Key == ModDataEntryName)
					{
						entryString = ZipUtils.GetModDataLine();
					}
					ZipUtils.WriteEntry(modDataSaveSlotFile, entry.Key, entryString);
				}
			}
		}
		DebugMsg("Cache Saved");
	}

	internal static void DebugMsg(string msg)
	{
		if (msg != null && ModDataManager.debugMode == true)
		{
			MelonLoader.MelonLogger.Msg(ConsoleColor.Yellow, msg);
		}
	}

}
