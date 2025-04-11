using System;
using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
{

    private GameObject[] spawnPoints;
    private Collider[] colliders;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
            colliders = GetComponentsInChildren<MeshCollider>();
            Respawn();

            MainMenuManager.instance.DisableButtons();
        }
        
    }

    public void Respawn()
    {
        if (!IsOwner) return;
        gameObject.SetActive(false);
        foreach(Collider col in colliders)
        {
            if (col.GetType() == typeof(SphereCollider)) continue;
            col.isTrigger = true;
        }

        int randomPoint = UnityEngine.Random.Range(0, spawnPoints.Length);
        transform.position = spawnPoints[randomPoint].transform.position;
        transform.rotation = spawnPoints[randomPoint].transform.rotation;

        gameObject.SetActive(true);
        foreach (Collider col in colliders)
        {
            if (col.GetType() == typeof(SphereCollider)) continue;
            col.isTrigger = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
