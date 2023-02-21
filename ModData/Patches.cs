
namespace ModData;

internal class ModDataPatches
{
	[HarmonyPatch(typeof(GameManager), nameof(GameManager.LoadSaveGameSlot), new Type[] { typeof(string), typeof(int) })]
	private static class ModData_GameManager_LoadSaveGameSlot
	{
		private static void Prefix(string slotName)
		{
			ModDataCore.InitModDataRoot();
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

	[HarmonyPatch(typeof(SaveGameSystem), nameof(SaveGameSystem.DeleteSaveFiles), new Type[] {typeof(string) })]
	private static class ModData_SaveGameSystem_DeleteSaveFiles
	{
		private static void Postfix(string name)
		{
			ModDataCore.CloseModDataSaveSlot();
			ModDataCore.DeleteModDataSaveSlot(name);
		}
	}
}