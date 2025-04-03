using Unity.Cinemachine;
using UnityEngine;

public class NetworkPlayer : MonoBehaviour
{

    private GameObject[] spawnPoints;
    private CinemachineCamera playerCam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        int randomPoint = Random.Range(0, spawnPoints.Length);
        transform.position = spawnPoints[randomPoint].transform.position;
        transform.rotation = spawnPoints[randomPoint].transform.rotation;

        playerCam = GameObject.FindWithTag("MainCamera").GetComponent<CinemachineCamera>();
        playerCam.Target.TrackingTarget = transform;

        MainMenuManager.instance.DisableButtons();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
