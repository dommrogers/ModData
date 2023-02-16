using MelonLoader;
using MelonLoader.Utils;


namespace ModData;

internal sealed class ModData : MelonMod
{
	public override void OnInitializeMelon()
	{
		//Any initialization code goes here.
		//This method can be deleted if no initialization is required.


		ModDataManager.InitModDataRoot();

	}
}
public static class ModDataManager
{

	readonly static string modDataFolderName = "ModData";
	private static string modDataRoot = "";
	private static string saveRoot = "";
	private static string modRoot = "";

	readonly static string globalFile = "Global";

	internal static void InitModDataRoot()
	{
		modDataRoot = Path.Combine(MelonEnvironment.ModsDirectory, modDataFolderName);
		if (!Directory.Exists(modDataRoot))
		{
			Directory.CreateDirectory(modDataRoot);
			MelonLogger.Warning("Creating modDataRoot folder (" + modDataRoot + ")");
		}
	}

	internal static void InitModSaveRoot(string saveSlotName)
	{
		saveRoot = Path.Combine(modDataRoot, saveSlotName);
		if (!Directory.Exists(saveRoot))
		{
			Directory.CreateDirectory(saveRoot);
			MelonLogger.Warning("Creating saveRoot folder (" + saveRoot + ")");
		}
	}

	internal static void InitModRoot(string modName)
	{
		modRoot = Path.Combine(saveRoot, modName);
		if (!Directory.Exists(modRoot))
		{
			Directory.CreateDirectory(modRoot);
			MelonLogger.Warning("Creating modRoot folder (" + modRoot + ")");
		}
	}
	/// <summary>
	/// 
	/// </summary>
	/// <param name="saveSlotName"></param>
	/// <param name="modName"></param>
	/// <param name="data"></param>
	/// <returns></returns>
	public static bool Save(string saveSlotName, string modName, string data)
	{
		return Save(saveSlotName, modName, data, globalFile, false);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="saveSlotName"></param>
	/// <param name="modName"></param>
	/// <param name="data"></param>
	/// <param name="useEncoding"></param>
	/// <returns></returns>
	public static bool Save(string saveSlotName, string modName, string data, bool useEncoding = false)
	{
		return Save(saveSlotName, modName, data, globalFile, useEncoding);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="saveSlotName"></param>
	/// <param name="modName"></param>
	/// <param name="data"></param>
	/// <param name="filename"></param>
	/// <returns></returns>
	public static bool Save(string saveSlotName, string modName, string data, string? filename = null)
	{
		return Save(saveSlotName, modName, data, filename, false);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="saveSlotName"></param>
	/// <param name="modName"></param>
	/// <param name="data"></param>
	/// <param name="filename"></param>
	/// <param name="useEncoding"></param>
	/// <returns></returns>
	public static bool Save(string saveSlotName, string modName, string data, string? filename = null, bool useEncoding = false)
	{
		if (filename == null)
		{
			filename = globalFile;
		}
		InitModSaveRoot(saveSlotName);
		InitModRoot(modName);
		string filePath = Path.Combine(modRoot, filename + ".moddata");

		if (useEncoding == true)
		{
			MelonLogger.Warning("Writing (encoded) to " + filePath);
			string encodedData = Base64Encode(data);
			File.WriteAllText(filePath, "MD64_" + encodedData, System.Text.Encoding.UTF8);
		}
		else
		{
			MelonLogger.Warning("Writing to " + filePath);
			File.WriteAllText(filePath, data, System.Text.Encoding.UTF8);
		}

		return true;
	}
	/// <summary>
	/// 
	/// </summary>
	/// <param name="saveSlotName"></param>
	/// <param name="modName"></param>
	/// <returns></returns>
	public static string? Load(string saveSlotName, string modName)
	{
		return Load(saveSlotName, modName, globalFile);
	}
	/// <summary>
	/// 
	/// </summary>
	/// <param name="saveSlotName"></param>
	/// <param name="modName"></param>
	/// <param name="filename"></param>
	/// <returns></returns>
	public static string? Load(string saveSlotName, string modName, string? filename = null)
	{
		string data = null;

		if (filename == null)
		{
			filename = globalFile;
		}
		InitModSaveRoot(saveSlotName);
		InitModRoot(modName);
		string filePath = Path.Combine(modRoot, filename + ".moddata");

		if (!File.Exists(filePath))
		{
			MelonLogger.Warning("File doesn't exist " + filePath);
			return null;
		}

		data = File.ReadAllText(filePath);


		if (data.StartsWith("MD64_"))
		{
			MelonLogger.Warning("Loaded (encoded) from " + filePath);
			return Base64Decode(data.Substring(5));
		}

		MelonLogger.Warning("Loaded from " + filePath);
		return data;
	}


	internal static string Base64Encode(string plainText)
	{
		var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
		return System.Convert.ToBase64String(plainTextBytes);
	}
	internal static string Base64Decode(string base64EncodedData)
	{
		var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
		return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
	}

}
