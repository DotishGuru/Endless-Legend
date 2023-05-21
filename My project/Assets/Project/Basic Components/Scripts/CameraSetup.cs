using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSetup : MonoBehaviour
{
    private CinemachineVirtualCamera vcam;
    private GameObject player;
    void Start()
    {
        vcam = this.gameObject.GetComponent<CinemachineVirtualCamera>();
        player = GameObject.FindGameObjectWithTag("Player");

        vcam.Follow = player.transform;
    }
}
