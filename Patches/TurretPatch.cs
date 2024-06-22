using HarmonyLib;
using MapHazardsMoves.Utils;
using UnityEngine;

namespace MapHazardsMoves.Patches
{
    [HarmonyPatch(typeof(Turret))]
    public class TurretPatch
    {
        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        private static void StartPatch(Turret __instance)
        {
            if(!__instance.IsServer) return;

            MapHazardsMoves.instance.RegisterHazardObject(__instance.NetworkObjectId);
        }
        
        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        private static void UpdatePatch(Turret __instance)
        {
            if(__instance.NetworkObjectId == null) return;
            if(!__instance.IsServer || !MapHazardsMoves.instance.turretEnabledEntry.Value) return;
            
            MapHazardsMoves.instance.OnUpdateHazardObject(__instance.NetworkObjectId, MapHazardsMoves.instance.turretSpeedEntry.Value, __instance.gameObject.transform.position);
            
        }
    }
}