using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.XR.Management;

public class SpawnPlayer : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject VRPlayer;

    [SerializeField] private Transform spawnPoint;
    void Start()
    {
        /*if(OpenVR.IsHmdPresent())
            Instantiate(VRPlayer, spawnPoint.position, Quaternion.identity);
        else*/
        Instantiate(player, spawnPoint.position, Quaternion.identity);
    }
}
