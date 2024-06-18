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
            if(!__instance.IsServer || __instance.hasExploded || !MapHazardsMoves.instance.landmineEnabledEntry.Value) return;
            
            HazardObject hazardObject = MapHazardsMoves.instance.HazardsObjects[__instance.NetworkObjectId];


            if(hazardObject == null) return;
            

            if (hazardObject.moveTimer > 0)
            {
                hazardObject.moveTimer -= Time.deltaTime;
            }
            else
            {
                hazardObject.moveTimer = MapHazardsMoves.instance.GetNewTimer();
                NetworkHazardsMoves.OnUpdateObjectClientRpc(
                    __instance.NetworkObjectId, 
                    MapHazardsMoves.instance.GetNewPos(__instance.transform.position),
                    MapHazardsMoves.instance.landmineSpeedEntry.Value
                );
            }


        }
    }
}