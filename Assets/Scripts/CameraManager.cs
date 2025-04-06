using System.Globalization;
using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : NetworkBehaviour
{
    private GameObject[] playerCams;
    private int activeCam = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (IsOwner)
        {
            playerCams = GameObject.FindGameObjectsWithTag("MainCamera");
            foreach (GameObject player in playerCams) 
            {
                player.GetComponent<CinemachineCamera>().Target.TrackingTarget = transform;
                player.SetActive(false);
            }
            playerCams[0].SetActive(true);
        }

    }

    public void OnSwitchCam(InputValue inputValue)
    {
        if (IsOwner)
        {
            activeCam++;
            if (activeCam >= playerCams.Length) activeCam = 0;

            for (int i = 0; i < playerCams.Length; i++)
            {
                if (i == activeCam)
                    playerCams[i].SetActive(true);
                else
                    playerCams[i].SetActive(false);
            }
        }
    }
}
