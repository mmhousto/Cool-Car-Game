using UnityEngine;

public class Smoking : MonoBehaviour
{

    public Health health;
    public ParticleSystem firePS;
    private ParticleSystem smokePS;
    ParticleSystem.MinMaxCurve startSize;
    ParticleSystem.MainModule main;
    private AudioSource explosion;
    private ParticleSystem explosionPS;


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
        if (health.health > 50 && smokePS.isPlaying || explosionPS.isPlaying)
        {
            smokePS.Stop();
            explosionPS.Stop();
        }

        if(health.health <= 50 && !smokePS.isPlaying) smokePS.Play(); // start smoke

        if(health.health <= 25 && main.startSize.constant != 0.75f) // Make smoke bigger
        {
            startSize.constant = 0.75f;
            main.startSize = startSize;
        }

        if (health.health <= 10 && main.startSize.constant != 1.0f) // Make Smoke full size
        {
            startSize.constant = 1.0f;
            main.startSize = startSize;
        }

        if(health.health <= 15 && !firePS.isPlaying) firePS.Play(); // start fire

        if(health.health <= 0)
        {
            explosion.transform.position = transform.position;
            explosion.Play();
            explosionPS.Play();
        }
    }
}
