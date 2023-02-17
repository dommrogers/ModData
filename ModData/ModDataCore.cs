
namespace ModData;

internal class ModDataCore
{
	readonly static string modDataFolderName = "ModData";
	readonly static string modDataFileExt = ".moddata";
	private static string modDataRoot = "";

	internal static string? modDataSaveSlotName = null;
	internal static string? modDataSaveSlotFile = null;

	internal static void InitModDataRoot()
	{
		modDataRoot = Path.Combine(MelonEnvironment.ModsDirectory, modDataFolderName);
		if (!Directory.Exists(modDataRoot))
		{
			Directory.CreateDirectory(modDataRoot);
			MelonLogger.Warning("Creating modDataRoot folder (" + modDataRoot + ")");
		}
	}

	internal static void InitModDataSaveSlot()
	{
		if (modDataSaveSlotName == null)
		{
			MelonLogger.Warning("No SaveSlot loaded.");
			return;
		}
		modDataSaveSlotFile = Path.Combine(modDataRoot, modDataSaveSlotName + modDataFileExt);

		// create and/or open the saveSlotFile
		if (!File.Exists(modDataSaveSlotFile))
		{
			ZipUtils.CreateEmptyFile(modDataSaveSlotFile);
			MelonLogger.Warning("Creating modDataSaveSlotFile (" + modDataSaveSlotFile + ")");
		}

	}

	internal static void CloseModDataSaveSlot()
	{
		modDataSaveSlotName = null;
		modDataSaveSlotFile = null;
	}


	internal static void WriteEntry(string entryName, string entryData, string? entrySuffix = null)
	{
		if (entrySuffix != null)
		{
			entryName += "_"+ entrySuffix;
		}
		if (modDataSaveSlotFile == null)
		{
			MelonLogger.Warning("No SaveSlot loaded.");
			return;
		}
		ZipUtils.WriteEntry(modDataSaveSlotFile, entryName, entryData);
	}

	internal static string? ReadEntry(string entryName, string? entrySuffix = null)
	{
		if (entrySuffix != null)
		{
			entryName += "_" + entrySuffix;
		}
		if (modDataSaveSlotFile == null)
		{
			MelonLogger.Warning("No SaveSlot loaded.");
			return null;
		}
		return ZipUtils.ReadEntry(modDataSaveSlotFile, entryName);
	}



}
