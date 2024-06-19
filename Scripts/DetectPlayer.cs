using System;
using MapHazardsMoves.Utils;
using Unity.Netcode;
using UnityEngine;

namespace MapHazardsMoves.Scripts;

public class DetectPlayer: MonoBehaviour
{
    public ulong networkId;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && networkId != null && other.gameObject.transform.position != null)
        {
            if (NetworkManager.Singleton.IsServer)
            {
                NetworkHazardsMoves.OnPlayerDetected(networkId, other.gameObject.transform.position);
            }
            else
            {
                NetworkHazardsMoves.OnPlayerDetectedServerRpc(networkId, other.gameObject.transform.position);
            }
        }
    }
}