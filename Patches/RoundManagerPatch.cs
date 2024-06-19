using HarmonyLib;
using UnityEngine;

namespace MapHazardsMoves.Patches;

[HarmonyPatch(typeof(RoundManager))]
public class RoundManagerPatch
{
    [HarmonyPatch(nameof(RoundManager.LoadNewLevel))]
    [HarmonyPrefix]
    private static void LoadLevelPatch(RoundManager __instance)
    {
        MapHazardsMoves.instance.HazardsObjects.Clear();
        if(MapHazardsMoves.instance.enableDevLogsEntry.Value) Debug.Log("MAP HAZARDS OBJECTS CLEANED");
    }
}