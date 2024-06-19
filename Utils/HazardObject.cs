using System;
using System.Collections;
using MapHazardsMoves.Scripts;
using UnityEngine;
using UnityEngine.AI;

namespace MapHazardsMoves.Utils
{
    public class HazardObject
    {
        public GameObject gameObject;
        public NavMeshAgent navMeshAgent;
        public DetectPlayer detectPlayer;
        public float moveTimer;
        public float detectPlayerTimer;

    }
}