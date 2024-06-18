using HarmonyLib;

namespace MapHazardsMoves.Patches;

[HarmonyPatch(typeof(RoundManager))]
public class RoundManagerPatch
{
    [HarmonyPatch(nameof(RoundManager.LoadNewLevel))]
    [HarmonyPrefix]
    private static void LoadLevelPatch(RoundManager __instance)
    {
        MapHazardsMoves.instance.HazardsObjects.Clear();
    }
}