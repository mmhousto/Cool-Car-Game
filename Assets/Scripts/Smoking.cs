using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Smoking : NetworkBehaviour
{

    public Health health;
    public ParticleSystem firePS;
    private ParticleSystem smokePS;
    ParticleSystem.MinMaxCurve startSize;
    ParticleSystem.MainModule main;
    private AudioSource explosion;
    private ParticleSystem explosionPS;
    public bool blewUp = false;
    private float power = 1500f;
    private float radius = 15f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        smokePS = GetComponent<ParticleSystem>();
        main = smokePS.main;
        startSize = main.startSize;
        explosion = GameObject.FindWithTag("Explosion").GetComponent<AudioSource>();
        explosionPS = explosion.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!NetworkObject.IsSpawned) return;

        if (health.health.Value > 50 && (smokePS.isPlaying || firePS.isPlaying))
        {
            smokePS.Stop();
            firePS.Stop();
        }

        if (health.health.Value <= 50 && !smokePS.isPlaying) smokePS.Play(); // start smoke

        if (health.health.Value <= 25 && main.startSize.constant != 0.75f) // Make smoke bigger
        {
            startSize.constant = 0.75f;
            main.startSize = startSize;
        }

        if (health.health.Value <= 10 && main.startSize.constant != 1.0f) // Make Smoke full size
        {
            startSize.constant = 1.0f;
            main.startSize = startSize;
        }

        if (health.health.Value <= 15 && !firePS.isPlaying) firePS.Play(); // start fire

        /*if(health.health.Value <= 0 && blewUp == false)
        {
            blewUp = true;
            BlowUpRpc();
        }*/
    }

    [Rpc(SendTo.Everyone)]
    public void BlowUpRpc()
    {
        explosion.transform.position = transform.position;
        explosion.Play();
        explosionPS.Play();
        Collider[] nearColliders = Physics.OverlapSphere(explosion.transform.position, radius);
        List<string> names = new List<string>();
        foreach (Collider collider in nearColliders)
        {
            if (collider.transform.root.TryGetComponent(out Rigidbody rigidbody))
            {
                
                if (collider.transform.root.TryGetComponent(out NetworkPlayer player) && names.Contains(player.GetComponentInChildren<PlayerName>(true).playerName.Value.ToString()) == false)
                {
                    PlayerName playerName = player.GetComponentInChildren<PlayerName>(true);
                    names.Add(playerName.playerName.Value.ToString());
                    Debug.Log("Blew Up: " + playerName.playerName.Value.ToString());
                    rigidbody.AddExplosionForce(power, explosion.transform.position, radius, power * 1.5f, ForceMode.Impulse);
                }
                else if (names.Contains(collider.transform.root.name) == false && player == null)
                {
                    names.Add(collider.transform.root.name);
                    Debug.Log("Blew Up: " + collider.transform.root.name);
                    rigidbody.AddExplosionForce(power, explosion.transform.position, radius, power * 1.5f, ForceMode.Impulse);
                }

            }
        }
    }
}
