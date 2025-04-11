using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class Health : NetworkBehaviour, IDamagable
{

    public NetworkVariable<int> health = new NetworkVariable<int>();
    public int maxHealth = 100;
    private Color startColor;
    public Renderer mainRenderer;
    private WaitForSeconds hitEffectTime;
    private NetworkPlayer np;
    private Smoking smoking;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hitEffectTime = new WaitForSeconds(0.15f);
        if (mainRenderer == null)
            mainRenderer = GetComponent<Renderer>();
        startColor = mainRenderer.material.color;
        
        if(IsOwner)
            health.Value = maxHealth;

        if (transform.tag == "Player")
        {
            np = GetComponent<NetworkPlayer>();
        }

        smoking = GetComponentInChildren<Smoking>();

        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectCallback;
    }

    private void OnDestroy()
    {
        // Since the NetworkManager can potentially be destroyed before this component, only
        // remove the subscriptions if that singleton still exists.
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnectedCallback;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnectCallback;
        }
    }

    private void OnClientConnectedCallback(ulong clientId)
    {
        if (!IsHost)
        {
            if (!NetworkObject.IsSpawned)
                Destroy(this.gameObject);
        }

    }

    private void OnClientDisconnectCallback(ulong clientId)
    {
        //OnClientConnectionNotification?.Invoke(clientId, ConnectionStatus.Disconnected);
    }


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsOwner)
            health.Value = maxHealth;

        health.OnValueChanged += OnStateChanged;
    }

    public override void OnNetworkDespawn()
    {
        health.OnValueChanged -= OnStateChanged;
    }

    public void OnStateChanged(int previous, int current)
    {
        /*if (health.Value <= 0)
        {
            if (transform.tag == "Player")
            {
                smoking.blewUp = false;
                smoking.BlowUpRpc();
                np.Respawn();
                health.Value = maxHealth;
            }
            else
            {
                if (smoking != null) smoking.BlowUpRpc();
                DestroyMeRpc();
            }
        }*/
    }

    [Rpc(SendTo.Everyone)]
    public void DamageRpc()
    {
        StartCoroutine(FlashRed());
        if (IsOwner)
            health.Value--;
        if (health.Value <= 0)
        {
            if (transform.tag == "Player")
            {
                smoking.blewUp = false;
                smoking.BlowUpRpc();
                np.Respawn();
                health.Value = maxHealth;
            }
            else
            {
                if (smoking != null) smoking.BlowUpRpc();
                DestroyMeRpc();
            }
               
        }

    }

    [Rpc(SendTo.Everyone)]
    public void DestroyMeRpc()
    {
        if (IsOwner)
        {
            NetworkObject networkObject = GetComponent<NetworkObject>();
            if (networkObject != null)
                networkObject.Despawn(); // use Despawn instead of Destroy in NGO
        }
    }

    IEnumerator FlashRed()
    {
        mainRenderer.material.color = Color.red;
        yield return hitEffectTime;
        mainRenderer.material.color = startColor;
    }
}
