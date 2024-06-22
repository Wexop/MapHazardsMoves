using System;
using MapHazardsMoves.Utils;
using Unity.Netcode;
using UnityEngine;

namespace MapHazardsMoves.Scripts;

public class DetectPlayer: MonoBehaviour
{
    public ulong? networkId = null;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && networkId.HasValue && other.gameObject.transform.position != null)
        {
            //Debug.Log($"PLAYER TRIGGER, IS SERVER {NetworkManager.Singleton.IsServer}, NETWORKID {networkId.Value}, NEW POS {other.gameObject.transform.position}");
            if (NetworkManager.Singleton.IsServer)
            {
                NetworkHazardsMoves.OnPlayerDetected(networkId.Value, other.gameObject.transform.position);
            }
        }
    }
}