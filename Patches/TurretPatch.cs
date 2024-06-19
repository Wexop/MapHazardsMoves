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
            
            NetworkHazardsMoves.RegisterObjectClientRpc(__instance.NetworkObjectId);
        }
        
        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        private static void UpdatePatch(Turret __instance)
        {
            if(!__instance.IsServer || !MapHazardsMoves.instance.turretEnabledEntry.Value) return;
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
                //Debug.Log("TURRET MOVE");
                //Debug.Log($"ID {__instance.NetworkObjectId} new pos {MapHazardsMoves.instance.GetNewPos(__instance.transform.position)} speed {MapHazardsMoves.instance.turretSpeedEntry.Value}");
                NetworkHazardsMoves.OnUpdateObjectClientRpc(
                    __instance.NetworkObjectId, 
                    MapHazardsMoves.instance.GetNewPos(__instance.transform.position),
                    MapHazardsMoves.instance.turretSpeedEntry.Value
                    );
            }


        }
    }
}