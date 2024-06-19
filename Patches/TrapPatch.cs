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
            
            NetworkHazardsMoves.RegisterObjectClientRpc(__instance.NetworkObjectId);
        }
        
        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        private static void UpdatePatch(SpikeRoofTrap __instance)
        {
            if(!__instance.IsServer || !MapHazardsMoves.instance.spikeTrapEnabledEntry.Value ) return;
            
            HazardObject hazardObject = MapHazardsMoves.instance.HazardsObjects[__instance.NetworkObjectId];


            if(hazardObject == null) return;
            
            if (hazardObject.detectPlayerTimer > 0)
            {
                hazardObject.detectPlayerTimer -= Time.deltaTime;
            }

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
                    MapHazardsMoves.instance.spikeTrapSpeedEntry.Value
                );
                
            }


        }
    }
}