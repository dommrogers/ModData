namespace ModData;

public class ModDataManager
{
	private string? currentModName = null;
	internal static bool debugMode { get; private set; } = false;

	public ModDataManager(string modName, bool debug = false)
	{
		currentModName = modName;
		debugMode = debug;
	}

	public bool Save(string data)
	{
		return Save(data, null);
	}

	public bool Save(string data, string? suffix)
	{
		if (currentModName == null)
		{
			throw new ArgumentNullException(nameof(currentModName));
		}
		bool saved = ModDataCore.WriteEntry(currentModName, data, suffix);
		return saved;
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