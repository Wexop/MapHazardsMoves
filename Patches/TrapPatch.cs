using HarmonyLib;
using MapHazardsMoves.Utils;
using UnityEngine;

namespace MapHazardsMoves.Patches
{
    [HarmonyPatch(typeof(SpikeRoofTrap))]
    public class SpikeRoofTrapPatch
    {
        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        private static void StartPatch(SpikeRoofTrap __instance)
        {
            
            if(!__instance.IsServer) return;
            
            MapHazardsMoves.instance.RegisterHazardObject(__instance.NetworkObjectId);
        }
        
        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        private static void UpdatePatch(SpikeRoofTrap __instance)
        {
            if(__instance.NetworkObjectId == null) return;
            if(!__instance.IsServer || !MapHazardsMoves.instance.spikeTrapEnabledEntry.Value) return;
            
            MapHazardsMoves.instance.OnUpdateHazardObject(__instance.NetworkObjectId, MapHazardsMoves.instance.spikeTrapSpeedEntry.Value, __instance.gameObject.transform.position);

        }
    }
}