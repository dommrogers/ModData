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

	public static bool Save(string data)
	{
		return Save(data, null);
	}

	public static bool Save(string data, string? suffix)
	{
		if (currentModName == null)
		{
			throw new ArgumentNullException(nameof(currentModName));
		}
		bool saved = ModDataCore.WriteEntry(currentModName, data, suffix);
		return saved;
	}

	public static string? Load()
	{
		return Load(null);
	}

	public static string? Load(string? suffix)
	{
		if (currentModName == null)
		{
			throw new ArgumentNullException(nameof(currentModName));
		}
		return ModDataCore.ReadEntry(currentModName, suffix);
	}


}