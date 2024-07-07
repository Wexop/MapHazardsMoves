using System;
using System.Collections;
using MapHazardsMoves.Scripts;
using UnityEngine;
using UnityEngine.AI;

namespace MapHazardsMoves.Utils
{
    public class HazardObject
    {
        public bool canWalk;
        public GameObject gameObject;
        public NavMeshAgent navMeshAgent;
        public DetectPlayer detectPlayer;
        public float moveTimer;
        public float detectPlayerTimer;
        public bool isStopped;
        public Vector3 centerPos = Vector3.up * 1.6f;
        public MapHazardTypes mapHazardType = MapHazardTypes.Other;

        public void Init()
        {
            if (mapHazardType == MapHazardTypes.SpikeTrap)
            {
                centerPos = new Vector3(0, 1, 1) * 1.6f;
            }
        }

        public Vector3 GetCenterPosition()
        {
            return gameObject.transform.position + centerPos;
        }

    }
}