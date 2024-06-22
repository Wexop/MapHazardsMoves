using System;
using System.Linq;
using MapHazardsMoves.Scripts;
using StaticNetcodeLib;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.AI;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace MapHazardsMoves.Utils
{
    [StaticNetcode]
    public class NetworkHazardsMoves
    { 
        [ClientRpc]
        public static void RegisterObjectClientRpc(ulong networkID)
        {
            var gameobjects = Object.FindObjectsByType<NetworkObject>(FindObjectsSortMode.None).ToList();
            var objectFound = gameobjects.Find(e => e.NetworkObjectId == networkID);

            if (objectFound == null)
            {
                if(MapHazardsMoves.instance.enableDevLogsEntry.Value) Debug.Log($"OBJECT NOT FOUND {networkID}");
            }
            else
            {

                NavMeshAgent navMeshAgent = objectFound.gameObject.AddComponent<NavMeshAgent>();
                DetectPlayer detectPlayer = objectFound.gameObject.AddComponent<DetectPlayer>();
                SphereCollider sphereCollider = objectFound.gameObject.AddComponent<SphereCollider>();
                sphereCollider.radius = MapHazardsMoves.instance.playerDetectionDistanceEntry.Value;
                sphereCollider.isTrigger = true;

                HazardObject hazardObject = new HazardObject();
                hazardObject.gameObject = objectFound.gameObject;
                hazardObject.moveTimer = 0;
                hazardObject.detectPlayerTimer = 0;
                hazardObject.navMeshAgent = navMeshAgent;
                hazardObject.navMeshAgent.height = 3.3f;
                hazardObject.navMeshAgent.radius = 1f;

                hazardObject.detectPlayer = detectPlayer;
                hazardObject.detectPlayer.networkId = networkID;
                
                
                MapHazardsMoves.instance.HazardsObjects.Add(networkID, hazardObject);

                if(MapHazardsMoves.instance.enableDevLogsEntry.Value) Debug.Log($"OBJECT ADDED {objectFound.name} WITH ID {networkID}");
            }
            
        }

        [ClientRpc]
        public static void OnUpdateObjectClientRpc(ulong networkId, Vector3 newPos, float speed)
        {
            //Debug.Log("ON UPDATE IS CALLED");
            //Debug.Log($"ON UPDATE IS CALLED WITH ID {networkId} newPos {newPos} speed {speed}");
            //Debug.Log($"MAP HAZRADS DICT {MapHazardsMoves.instance.HazardsObjects}");
            //Debug.Log($"CONTAINS KEY {MapHazardsMoves.instance.HazardsObjects?.ContainsKey(networkId)}");

            if(newPos == null || networkId == null || speed == null) return;
            
            if (!MapHazardsMoves.instance.HazardsObjects.ContainsKey(networkId))
            {
                Debug.LogError($"No HazardObject found with networkId {networkId}");
                return;
            }
            
            HazardObject hazardObject = MapHazardsMoves.instance.HazardsObjects[networkId];

            if(hazardObject == null) return;
            
            if(hazardObject.navMeshAgent == null) return;
            
            hazardObject.navMeshAgent.speed = speed;
            hazardObject.navMeshAgent.SetDestination(newPos);

            if(MapHazardsMoves.instance.enableDevLogsEntry.Value) Debug.Log($"HAZARD MOVED {hazardObject.gameObject.name}");
        }
        
        public static void OnPlayerDetected(ulong networkId, Vector3 newPos)
        {
            
            if(MapHazardsMoves.instance.enableDevLogsEntry.Value) Debug.Log($"PLAYER DETECTED WITH id {networkId} pos: {newPos}");
            
            if(newPos == null || !MapHazardsMoves.instance.enablePlayerDetectionEntry.Value || networkId == null) return;
            
            if (!MapHazardsMoves.instance.HazardsObjects.ContainsKey(networkId))
            {
                Debug.LogError($"No HazardObject found with networkId {networkId}");
                return;
            }
            
            HazardObject hazardObject = MapHazardsMoves.instance.HazardsObjects[networkId];
            
            if(hazardObject == null || hazardObject.detectPlayerTimer > 0) return;

            hazardObject.detectPlayerTimer = MapHazardsMoves.instance.playerDetectionDelayEntry.Value;
            hazardObject.moveTimer = MapHazardsMoves.instance.GetNewTimer();
            OnUpdateObjectClientRpc(networkId, newPos, MapHazardsMoves.instance.playerDetectionSpeedEntry.Value);
            
            if(MapHazardsMoves.instance.enableDevLogsEntry.Value) Debug.Log($"HAZARD DETECTED PLAYER {hazardObject.gameObject.name}");
        }
        
    }
}