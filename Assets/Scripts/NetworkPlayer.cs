using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
{

    private GameObject[] spawnPoints;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");

            Respawn();

            MainMenuManager.instance.DisableButtons();
        }
        
    }

    public void Respawn()
    {
        if (!IsOwner) return;
        gameObject.SetActive(false);
        int randomPoint = Random.Range(0, spawnPoints.Length);
        transform.position = spawnPoints[randomPoint].transform.position;
        transform.rotation = spawnPoints[randomPoint].transform.rotation;
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
