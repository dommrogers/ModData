namespace ModData;

internal class ZipUtils
{
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
				ZipArchiveEntry modDataEntry = archive.CreateEntry(ModDataCore.ModDataEntryName);
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
	/// <param name="entryName"></param>
	/// <param name="entryData"></param>
	/// <exception cref="ArgumentNullException"></exception>
	internal static void WriteEntry(string filePath, string entryName, string entryData)
	{
		if (filePath == null)
		{
			throw new ArgumentNullException(nameof(filePath));
		}
		if (entryName == null)
		{
			throw new ArgumentNullException(nameof(entryName));
		}

		filePath = SanitizeFilePath(filePath);
		entryName = SanitizeEntryName(entryName);

		if (!File.Exists(filePath))
		{
			CreateEmptyFile(filePath);
		}

		using (FileStream zipToOpen = new FileStream(filePath, FileMode.Open))
		{
			using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
			{
				if (archive.GetEntry(entryName) != null)
				{
					archive.GetEntry(entryName).Delete();
				}
				ZipArchiveEntry modDataEntry = archive.CreateEntry(entryName);
				using (StreamWriter writer = new StreamWriter(modDataEntry.Open()))
				{
					writer.Write(entryData);
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
	/// <param name="entryName"></param>
	/// <returns></returns>
	/// <exception cref="ArgumentNullException"></exception>
	internal static string? ReadEntry(string filePath, string entryName)
	{
		if (filePath == null)
		{
			throw new ArgumentNullException(nameof(filePath));
		}
		if (entryName == null)
		{
			throw new ArgumentNullException(nameof(entryName));
		}

		filePath = SanitizeFilePath(filePath);
		entryName = SanitizeEntryName(entryName);

		if (!File.Exists(filePath))
		{
			CreateEmptyFile(filePath);
		}

		using (FileStream zipToOpen = new FileStream(filePath, FileMode.Open))
		{
			using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read))
			{
				ZipArchiveEntry? modDataEntry = archive.GetEntry(entryName);
				if (modDataEntry == null)
				{
					return null;
				}
				using (StreamReader reader = new StreamReader(modDataEntry.Open()))
				{
					string? result = reader.ReadToEnd();
					reader.Dispose();
					archive.Dispose();
					zipToOpen.Dispose();
					return result;
				}
			}
		}
	}
	
	/// <summary>
	/// 
	/// </summary>
	/// <param name="filePath"></param>
	/// <returns></returns>
	/// <exception cref="ArgumentNullException"></exception>
	internal static List<string> GetEntries(string filePath)
	{
		List<string> list = new();

		if (filePath == null)
		{
			throw new ArgumentNullException(nameof(filePath));
		}

		filePath = SanitizeFilePath(filePath);

		if (!File.Exists(filePath))
		{
			CreateEmptyFile(filePath);
		}

		using (FileStream zipToOpen = new FileStream(filePath, FileMode.Open))
		{
			using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read))
			{
				foreach (ZipArchiveEntry? modDataEntry in archive.Entries)
				{
					if (modDataEntry != null)
					{
						if (modDataEntry.Name != null)
						{
							list.Add(modDataEntry.Name);
						}
					}
				}
			}
		}

		return list;
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
