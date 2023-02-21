namespace ModData;

internal sealed class Main : MelonMod
{
	public override void OnInitializeMelon()
	{
		MelonLogger.Msg("ModData Initialized - " + BuildInfo.ModVersion);
	}
}