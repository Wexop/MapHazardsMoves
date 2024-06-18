﻿using System;
using System.Linq;
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

                objectFound.gameObject.AddComponent<NavMeshAgent>();

                HazardObject hazardObject = new HazardObject();
                hazardObject.gameObject = objectFound.gameObject;
                hazardObject.moveTimer = 0;
                hazardObject.navMeshAgent = objectFound.GetComponent<NavMeshAgent>();
                hazardObject.navMeshAgent.height = 3.3f;
                hazardObject.navMeshAgent.radius = 1f;
                MapHazardsMoves.instance.HazardsObjects.Add(networkID, hazardObject);

                if(MapHazardsMoves.instance.enableDevLogsEntry.Value) Debug.Log($"OBJECT ADDED {objectFound.name} WITH ID {networkID}");
            }
            
        }

        [ClientRpc]
        public static void OnUpdateObjectClientRpc(ulong networkId, Vector3 newPos, float speed)
        {

            if(newPos == null) return;
            
            HazardObject hazardObject = MapHazardsMoves.instance.HazardsObjects[networkId];

            if(hazardObject == null) return;
            
            hazardObject.navMeshAgent.speed = speed;
            hazardObject.navMeshAgent.SetDestination(newPos);

            if(MapHazardsMoves.instance.enableDevLogsEntry.Value) Debug.Log($"HAZARD MOVED {hazardObject.gameObject.name}");
        }
        
    }
}