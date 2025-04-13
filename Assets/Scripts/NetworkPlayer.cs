using System;
using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class NetworkPlayer : NetworkBehaviour
{

    private GameObject[] spawnPoints;
    private Collider[] colliders;
    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
            colliders = GetComponentsInChildren<MeshCollider>();
            rb = GetComponent<Rigidbody>();
            Respawn();

            MainMenuManager.instance.DisableButtons();

            PauseManager.instance.resetButton.onClick.AddListener(Respawn);
        }
        
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        if (IsOwner)
        {
            PauseManager.instance.resetButton.onClick.RemoveListener(Respawn);
        }
    }

    public void Respawn()
    {
        if (!IsOwner) return;
        rb.linearVelocity = Vector3.zero;
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

    public void OnPause(InputValue inputValue)
    {
        if (!IsOwner) return;
        PauseManager.instance.PauseResume();
    }
}
