namespace ModData;

public class ModDataManager
{
	private static string? currentModName = null;
	internal static bool debugMode { get; private set; } = false;

	public ModDataManager(string modName, bool debug = false)
	{
		currentModName = modName;
		debugMode = debug;
	}

	public void Save(string data)
	{
		Save(data, null);
	}

	public void Save(string data, string? suffix)
	{
		if (currentModName == null)
		{
			throw new ArgumentNullException(nameof(currentModName));
		}
		ModDataCore.WriteEntry(currentModName, data, suffix);
	}

	public string? Load()
	{
		return Load(null);
	}

	public string? Load(string? suffix)
	{
		if (currentModName == null)
		{
			throw new ArgumentNullException(nameof(currentModName));
		}
		return ModDataCore.ReadEntry(currentModName, suffix);
	}


}