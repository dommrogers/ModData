using System.Diagnostics;

namespace ModData;

internal class ModDataCore
{
	internal static string modDataFolderName { get; } = "ModData";
	internal static string modDataFileExt { get; } = ".moddata";
	internal static string modDataRoot { get; private set; } = "";

	internal static string? modDataSaveSlotName { get; private set; } = null;
	internal static string? modDataSaveSlotFile { get; private set; } = null;

	internal static Dictionary<string, string> dataCache = new();

	internal static bool initComplete = false;
	internal static bool cacheLoaded = false;
	internal static bool cacheChanged = false;

	internal static void InitModDataCore(string saveName)
	{
		if (initComplete && cacheLoaded)
		{
			return;
		}
		InitModDataRoot();
		DebugMsg($"Init ModData: {saveName}");
		modDataSaveSlotName = saveName;
		InitModDataSaveSlot();
		initComplete = true;
		LoadCache();
		cacheLoaded = true;


	}

	internal static void InitModDataRoot()
	{
		modDataRoot = Path.Combine(MelonEnvironment.ModsDirectory, modDataFolderName);
		if (!Directory.Exists(modDataRoot))
		{
			Directory.CreateDirectory(modDataRoot);
//			DebugMsg($"InitModDataRoot: {modDataRoot}");
		}
	}

	internal static void InitModDataSaveSlot()
	{
		if (modDataSaveSlotName == null)
		{
			//DebugMsg("No SaveSlot loaded.");
			return;
		}
		modDataSaveSlotFile = Path.Combine(modDataRoot, modDataSaveSlotName + modDataFileExt);
//		DebugMsg($"InitModDataSaveSlot: {modDataSaveSlotFile}");
	}

	internal static void DeleteModDataSaveSlot(string slotName)
	{
		InitModDataRoot();

		modDataSaveSlotFile = Path.Combine(modDataRoot, slotName + modDataFileExt);

		if (File.Exists(modDataSaveSlotFile))
		{
			DebugMsg($"Delete: {modDataSaveSlotFile}");
			File.Delete(modDataSaveSlotFile);
		}

	}

	internal static void CloseModDataSaveSlot()
	{
		if (initComplete)
		{
			modDataSaveSlotName = null;
			modDataSaveSlotFile = null;
			dataCache.Clear();
			initComplete = false;
			cacheLoaded = false;
//			DebugMsg($"CloseModDataSaveSlot: {modDataSaveSlotFile}");
		}
	}


	internal static bool WriteEntry(string entryName, string entryData, string? entrySuffix = null)
	{
		if (entrySuffix != null)
		{
			entryName += "_" + entrySuffix;
		}
		if (!cacheLoaded)
		{
//			DebugMsg($"WriteEntry: No Cache. {entryName}");
			return false;
		}


		if (dataCache.ContainsKey(entryName) && dataCache[entryName] == entryData)
		{
//			DebugMsg($"WriteEntry: {entryName} {entryData.Length} NO CHANGE");
			return true;
		}

		if (dataCache.ContainsKey(entryName))
		{
			dataCache.Remove(entryName);
		}

		dataCache.Add(entryName, entryData);
//		DebugMsg($"WriteEntry: {entryName} {entryData.Length} {entryData.Trim().Length}");

		cacheChanged = true;
		return true;
	}

	internal static string? ReadEntry(string entryName, string? entrySuffix = null)
	{
		if (entrySuffix != null)
		{
			entryName += "_" + entrySuffix;
		}
		if (!cacheLoaded)
		{
//			DebugMsg($"ReadEntry: No Cache. {entryName}");
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
		Stopwatch sw = new Stopwatch();
		sw.Start();

		Dictionary<string, string> entries = ZipUtils.ReadEntries(modDataSaveSlotFile);
		foreach (var entry in entries)
		{
//			DebugMsg("Cache Loaded (" + entry.Key + ")");
			if (entry.Value != null)
			{
				dataCache.Add(entry.Key, entry.Value);
			}
		}

		cacheLoaded = true;
		sw.Stop();
		DebugMsg($"Cache Loaded ({entries.Count}) ({sw.ElapsedMilliseconds}ms)");
	}

	internal static void SaveCache()
	{
		if (!cacheLoaded)
		{
//			DebugMsg("SaveCache: No Cache.");
			return;
		}
		if (!cacheChanged)
		{
			return;
		}
		Stopwatch sw = new Stopwatch();
		sw.Start();
		if (dataCache != null && dataCache.Count > 0)
		{
			ZipUtils.WriteEntries(modDataSaveSlotFile, dataCache);
		}
		sw.Stop();
		DebugMsg($"Cache Saved ({dataCache.Count}) ({sw.ElapsedMilliseconds}ms)");
		cacheChanged = false;
	}

	internal static void DebugMsg(string msg)
	{
		//if (msg != null && ModDataManager.debugMode == true)
		//{
		MelonLoader.MelonLogger.Msg(ConsoleColor.Yellow, msg);
		//}
	}

}
