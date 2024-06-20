using HarmonyLib;
using MapHazardsMoves.Utils;
using UnityEngine;

namespace MapHazardsMoves.Patches
{
    [HarmonyPatch(typeof(Landmine))]
    public class LandminePatch
    {
        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        private static void StartPatch(Landmine __instance)
        {
            
            if(!__instance.IsServer) return;
            
            NetworkHazardsMoves.RegisterObjectClientRpc(__instance.NetworkObjectId);
        }
        
        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        private static void UpdatePatch(Landmine __instance)
        {
            if(__instance.NetworkObjectId == null) return;
            if(!__instance.IsServer || !MapHazardsMoves.instance.landmineEnabledEntry.Value) return;
            
            MapHazardsMoves.instance.OnUpdateHazardObject(__instance.NetworkObjectId, MapHazardsMoves.instance.landmineSpeedEntry.Value, __instance.gameObject.transform.position);



        }
    }
}