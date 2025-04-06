using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class Health : NetworkBehaviour, IDamagable
{

    public int health;
    public int maxHealth = 100;
    private Color startColor;
    public Renderer mainRenderer;
    private WaitForSeconds hitEffectTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hitEffectTime = new WaitForSeconds(0.15f);
        if(mainRenderer == null )
            mainRenderer = GetComponent<Renderer>();
        startColor = mainRenderer.material.color;
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    [Rpc(SendTo.Everyone)]
    public void DamageRpc()
    {
        StartCoroutine(FlashRed());
        health--;
        if(health < 0)
        {
            if (transform.tag == "Player")
            {
                GetComponent<NetworkPlayer>().Respawn();
                health = maxHealth;
            }
            else if (IsOwner)
                NetworkObject.Destroy(gameObject);
            else
                Destroy(gameObject);
        }
            
    }

    IEnumerator FlashRed()
    {
        mainRenderer.material.color = Color.red;
        yield return hitEffectTime;
        mainRenderer.material.color = startColor;
    }
}
