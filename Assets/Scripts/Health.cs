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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hitEffectTime = new WaitForSeconds(0.15f);
        if (mainRenderer == null)
            mainRenderer = GetComponent<Renderer>();
        startColor = mainRenderer.material.color;

        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnNetworkSpawn()
    {
        if(IsOwner)
            health.Value = maxHealth;

        if (health.Value <= 0)
        {
            if (transform.tag == "Player")
            {
                GetComponent<NetworkPlayer>().Respawn();
                health.Value = maxHealth;
            }
            else DestroyMeRpc();
        }

        

        health.OnValueChanged += OnStateChanged;
    }

    public override void OnNetworkDespawn()
    {
        health.OnValueChanged -= OnStateChanged;
    }

    public void OnStateChanged(int previous, int current)
    {
        if (health.Value <= 0)
        {
            if (transform.tag == "Player")
            {
                GetComponent<NetworkPlayer>().Respawn();
                health.Value = maxHealth;
            }
            else
                DestroyMeRpc();
        }
    }

    [Rpc(SendTo.Everyone)]
    public void DamageRpc()
    {
        StartCoroutine(FlashRed());
        if(IsOwner)
            health.Value--;
        if(health.Value <= 0)
        {
            if (transform.tag == "Player")
            {
                GetComponent<NetworkPlayer>().Respawn();
                health.Value = maxHealth;
            }
            else
                DestroyMeRpc();
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
