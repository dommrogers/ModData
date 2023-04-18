namespace ModData;

internal class ModDataPatches
{
	[HarmonyPatch(typeof(SaveGameSlots), nameof(SaveGameSlots.CreateSlot), new Type[] { typeof(string) , typeof(SaveSlotType) , typeof(uint) , typeof(Episode) })]
	private static class ModData_SaveGameSlots_CreateSaveSlotInfo
	{
		private static void Prefix(string slotname, SaveSlotType gameMode, uint gameId, Episode episode)
		{
			if (slotname != null && slotname != SaveGameSlots.AUTOSAVE_SLOT_NAME && slotname != SaveGameSlots.QUICKSAVE_SLOT_PREFIX)
			{
				ModDataCore.InitModDataCore(slotname);
			}
		}
	}
	[HarmonyPatch(typeof(GameManager), nameof(GameManager.LoadSaveGameSlot), new Type[] { typeof(string), typeof(int) })]
	private static class ModData_GameManager_LoadSaveGameSlot
	{
		private static void Prefix(string slotName, int saveChangelistVersion)
		{
			if (slotName != null && slotName != SaveGameSlots.AUTOSAVE_SLOT_NAME && slotName != SaveGameSlots.QUICKSAVE_SLOT_PREFIX)
			{
				ModDataCore.InitModDataCore(slotName);
			}
		}
	}
	[HarmonyPatch(typeof(GameManager), nameof(GameManager.LoadSaveGameSlot), new Type[] { typeof(SaveSlotInfo) })]
	private static class ModData_GameManager_LoadSaveGameSlot_ssi
	{
		private static void Prefix(SaveSlotInfo ssi)
		{
			string slotName = ssi.m_SaveSlotName;
			if (slotName != null && slotName != SaveGameSlots.AUTOSAVE_SLOT_NAME && slotName != SaveGameSlots.QUICKSAVE_SLOT_PREFIX)
			{
				ModDataCore.InitModDataCore(slotName);
			}
		}
	}
	[HarmonyPatch(typeof(SaveGameSystem), nameof(SaveGameSystem.SaveCompletedInternal), new Type[] { typeof(bool)})]
	private static class ModData_SaveGameSystem_SaveCompletedInternal
	{
		private static void Postfix()
		{
			ModDataCore.SaveCache();
		}
	}

	[HarmonyPatch(typeof(GameManager), nameof(GameManager.DoExitToMainMenu))]
	[HarmonyPatch(typeof(GameManager), nameof(GameManager.LoadMainMenu))]
	[HarmonyPatch(typeof(GameManager), nameof(GameManager.AsyncLoadMainMenu))]
	private static class ModData_GameManager_MainMenu
	{
		private static void Postfix()
		{
			ModDataCore.SaveCache();
			ModDataCore.CloseModDataSaveSlot();
		}
	}

	[HarmonyPatch(typeof(SaveGameSystem), nameof(SaveGameSystem.DeleteSaveFiles), new Type[] { typeof(string) })]
	private static class ModData_SaveGameSystem_DeleteSaveFiles
	{
		private static void Postfix(string name)
		{
			ModDataCore.CloseModDataSaveSlot();
			ModDataCore.DeleteModDataSaveSlot(name);
		}
	}
}