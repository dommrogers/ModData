
namespace ModData;

internal class ModDataPatches
{
	[HarmonyPatch(typeof(GameManager), nameof(GameManager.LoadSaveGameSlot), new Type[] { typeof(string), typeof(int) })]
	private static class ModData_GameManager_LoadSaveGameSlot
	{
		private static void Prefix(string slotName)
		{
			ModDataCore.modDataSaveSlotName = slotName;
			ModDataCore.InitModDataSaveSlot();
		}
	}

	[HarmonyPatch(typeof(GameManager), nameof(GameManager.DoExitToMainMenu))]
	private static class ModData_GameManager_DoExitToMainMenu
	{
		private static void Postfix()
		{
			ModDataCore.CloseModDataSaveSlot();
		}
	}
}