namespace ModData;

internal class ZipUtils
{

	static FileStream? fileStream;
	static ZipArchive? zipArchive;

	internal static string ModDataEntryName { get; } = "ModData.txt";


	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	internal static string GetModDataLine()
	{
		return "Mod Data | " + BuildInfo.ModVersion + " | " + DateTime.Now.ToString("yyyy-MM-dd HH:mm");
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="filePath"></param>
	/// <exception cref="ArgumentNullException"></exception>
	internal static void CreateEmptyFile(string? filePath)
	{
//		ModDataCore.DebugMsg($"CreateEmptyFile: {filePath}");
		if (filePath == null)
		{
			throw new ArgumentNullException(nameof(filePath));
		}

		filePath = SanitizeFilePath(filePath);

		if (File.Exists(filePath))
		{
			return;
		}
		using (FileStream zipToOpen = new FileStream(filePath, FileMode.Create))
		{
			using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
			{
				ZipArchiveEntry modDataEntry = archive.CreateEntry(ModDataEntryName);
				using (StreamWriter writer = new StreamWriter(modDataEntry.Open()))
				{
					writer.WriteLine(GetModDataLine());
					writer.Dispose();
				}
				archive.Dispose();
				zipToOpen.Dispose();
			}
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="filePath"></param>
	/// <param name="entries">Dictionary<string, string></param>
	/// <exception cref="ArgumentNullException"></exception>
	internal static void WriteEntries(string filePath, Dictionary<string, string> entries)
	{
		LoadArchive(filePath);

		entries[ModDataEntryName] = GetModDataLine();
		foreach (KeyValuePair<string, string> e in entries)
		{
			string entryName = e.Key;
			if (entryName == null)
			{
				throw new ArgumentNullException(nameof(entryName));
			}
			entryName = SanitizeEntryName(entryName);
			if (zipArchive.GetEntry(entryName) != null)
			{
				zipArchive.GetEntry(entryName).Delete();
//				ModDataCore.DebugMsg($"WriteEntries->Delete: {filePath} {entryName}");
			}
			string entryData = e.Value;
			ZipArchiveEntry modDataEntry = zipArchive.CreateEntry(entryName);
			using (StreamWriter writer = new StreamWriter(modDataEntry.Open()))
			{
				writer.Write(entryData);
				writer.Dispose();
//				ModDataCore.DebugMsg($"WriteEntries->Write: {entryName} {entryData.Length}");
			}
		}
		CloseArchive();
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="filePath"></param>
	/// <returns></returns>
	internal static Dictionary<string,string> ReadEntries(string filePath)
	{
		var entries = new Dictionary<string, string>();
		LoadArchive(filePath);

		foreach (var entry in zipArchive.Entries)
		{
			if(entry != null)
			{
				using (StreamReader reader = new StreamReader(entry.Open()))
				{
					string? result = reader.ReadToEnd();
					reader.Dispose();
					entries.Add(entry.Name,result);
				}
			}
		}

		CloseArchive();
		return entries;
	}

	internal static void LoadArchive(string filePath)
	{
//		ModDataCore.DebugMsg($"LoadArchive: {filePath} {zipArchive != null}");
		if (zipArchive != null)
		{
			return;
		}
		if (filePath == null)
		{
			throw new ArgumentNullException(nameof(filePath));
		}

		filePath = SanitizeFilePath(filePath);

		if (!File.Exists(filePath))
		{
			CreateEmptyFile(filePath);
		}

		fileStream = new FileStream(filePath, FileMode.Open);
		zipArchive = new ZipArchive(fileStream, ZipArchiveMode.Update);
	}

	internal static void CloseArchive()
	{
//		ModDataCore.DebugMsg($"CloseArchive");
		zipArchive.Dispose();
		fileStream.Dispose();
		zipArchive = null;
		fileStream = null;
	}

	// placeholders, may not ever need them
	internal static string SanitizeFilePath(string filePath)
	{
		return filePath;
	}
	internal static string SanitizeEntryName(string entryName)
	{
		return entryName;
	}


}
