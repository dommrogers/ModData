
namespace ModData;

internal class ZipUtils
{

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
				ZipArchiveEntry modDataEntry = archive.CreateEntry("ModData.txt");
				using (StreamWriter writer = new StreamWriter(modDataEntry.Open()))
				{
					writer.WriteLine("Mod Data : " + BuildInfo.ModVersion);
					writer.Dispose();
				}
				archive.Dispose();
				zipToOpen.Dispose();
			}
		}
	}

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

	internal static string SanitizeFilePath(string filePath)
	{

		// TODO ?
		return filePath;
	}
	internal static string SanitizeEntryName(string entryName)
	{
		return entryName.Replace(" ", "-").Replace("&", "").Replace("\\", "");
	}


}
