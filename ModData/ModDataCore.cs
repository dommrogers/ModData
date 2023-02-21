
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
			MelonLogger.Msg(ConsoleColor.Cyan,"Creating modDataRoot (" + modDataRoot + ")");
			Directory.CreateDirectory(modDataRoot);
		}
	}

	internal static void InitModDataSaveSlot()
	{
		if (modDataSaveSlotName == null)
		{
			MelonLogger.Msg(ConsoleColor.Cyan, "No SaveSlot loaded.");
			return;
		}
		modDataSaveSlotFile = Path.Combine(modDataRoot, modDataSaveSlotName + modDataFileExt);

		// create and/or open the saveSlotFile
		if (!File.Exists(modDataSaveSlotFile))
		{
			MelonLogger.Msg(ConsoleColor.Cyan, "Creating modDataSaveSlotFile (" + modDataSaveSlotFile + ")");
			ZipUtils.CreateEmptyFile(modDataSaveSlotFile);
		}
	}

	internal static void DeleteModDataSaveSlot(string slotName)
	{
		InitModDataRoot();

		modDataSaveSlotFile = Path.Combine(modDataRoot, slotName + modDataFileExt);

		if (File.Exists(modDataSaveSlotFile))
		{
			MelonLogger.Msg(ConsoleColor.Cyan, "Deleting modDataSaveSlotFile (" + modDataSaveSlotFile + ")");
			File.Delete(modDataSaveSlotFile);
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
			entryName += "_" + entrySuffix;
		}
		if (modDataSaveSlotFile == null)
		{
			MelonLogger.Msg(ConsoleColor.Cyan, "No SaveSlot loaded.");
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
			MelonLogger.Msg(ConsoleColor.Cyan, "No SaveSlot loaded.");
			return null;
		}
		return ZipUtils.ReadEntry(modDataSaveSlotFile, entryName);
	}



}
