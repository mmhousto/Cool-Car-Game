using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GatlingGun : NetworkBehaviour
{
    // target the gun will aim at
    Transform go_target;

    // Gameobjects need to control rotation and aiming
    public Transform go_baseRotation;
    public Transform go_GunBody;
    public Transform go_barrel;

    // Gun barrel rotation
    public float barrelRotationSpeed;
    float currentRotationSpeed;

    // Distance the turret can aim and fire from
    public float firingRange;

    public float hitBackMultiplier = 1f;

    // Particle system for the muzzel flash
    public ParticleSystem muzzelFlash;

    // Overheat Bar
    private Slider fireTimeSlider;

    // Audio source and clips for firing gatling gun
    public AudioSource gunAudioSource;
    public AudioClip firingSound, endSound;

    // Audio source for bullet hit sound
    public AudioSource bulletHitAudioSource;

    // Layers can hit
    LayerMask layerMask;

    // Used to start and stop the turret firing
    bool canFire = false;
    bool firing = false;
    bool startedFiring = false;
    float fireMaxTime = 5f;
    [Range(0f, 5f)]
    float fireTime = 0;
    float fireRate = 0.1f;
    float coolDown = 0;

    Coroutine shooting = null;

    void Start()
    {
        if (IsOwner)
        {
            bulletHitAudioSource = GameObject.FindWithTag("BulletHit").GetComponent<AudioSource>();
            fireTimeSlider = GameObject.FindWithTag("FireTime").GetComponent<Slider>();
            layerMask = LayerMask.GetMask("Default", "Player");
            // Set the firing range distance
            this.GetComponent<SphereCollider>().radius = firingRange;
            canFire = true;
            coolDown = 0;
            fireTime = fireMaxTime;
            fireTimeSlider.value = fireTime;
        }
        
    }

    void Update()
    {
        if (IsOwner)
        {
            HandleCanFire();


            AimAndFire();
        }
        
    }

    private void FixedUpdate()
    {
        if (IsOwner)
        {
            Shoot();
        }
        
    }

    void OnDrawGizmosSelected()
    {
        // Draw a red sphere at the transform's position to show the firing range
        Gizmos.color = Color.red;
        Gizmos.DrawLine(go_barrel.transform.position, go_barrel.position + go_barrel.forward * 100);
    }

    void HandleCanFire()
    {
        if (fireTime <= 0 && coolDown <= 0) // reset cooldown
        {
            coolDown = 2.5f;
        }

        if (coolDown > 0) // decrease cooldown time
        {
            if (canFire != false)
                canFire = false;
            if (firing != false)
                firing = false;
            coolDown -= Time.deltaTime;
            if (coolDown <= 0) fireTime = 0.01f;
        }

        if (coolDown <= 0 && fireTime > 0) // if cooldown ended and fire time is greater than 0, then canFire
        {
            canFire = true;
        }


    }

    void AimAndFire()
    {
        fireTimeSlider.value = fireTime;

        // Gun barrel rotation
        go_barrel.transform.Rotate(0, 0, currentRotationSpeed * Time.deltaTime);

        // if can fire turret activates
        if (canFire && firing && fireTime > 0) // firing 
        {
            fireTime -= Time.deltaTime;
            // start rotation
            currentRotationSpeed = barrelRotationSpeed;


            // start particle system 
            if (!muzzelFlash.isPlaying) muzzelFlash.Play();
        }
        else
        {
            // slow down barrel rotation and stop
            currentRotationSpeed = Mathf.Lerp(currentRotationSpeed, 0, 10 * Time.deltaTime);

            // stop the particle system
            if (muzzelFlash.isPlaying)
            {
                muzzelFlash.Stop();
            }
        }

        if (coolDown <= 0 && firing == false && fireTime < fireMaxTime) // increase fire time if not firing and not in cooldown
        {
            fireTime += Time.deltaTime;
        }

        if (fireTime > fireMaxTime) fireTime = fireMaxTime; // set fire time to max
    }

    void Shoot()
    {
        if (canFire && firing && fireTime > 0 && shooting == null && startedFiring == false && IsOwner) // firing 
        {
            // Start Audio Source
            gunAudioSource.Play();
            shooting = StartCoroutine(Shooting());
            startedFiring = true;
            Debug.Log("Shooting Coroutine Started");
        }
    }

    IEnumerator Shooting()
    {
        startedFiring = true;

        while (firing)
        {

            RaycastHit hit;

            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(go_barrel.position, go_barrel.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
            {
                Debug.Log(hit.transform.name);
                bulletHitAudioSource.transform.position = hit.transform.position;
                bulletHitAudioSource.PlayOneShot(bulletHitAudioSource.clip);

                if (hit.transform.TryGetComponent(out IPushable pushable))
                {
                    pushable.Push(-hit.normal, hitBackMultiplier);
                }

                if (hit.transform.TryGetComponent(out IDamagable damagable))
                {
                    damagable.Damage();
                }
            }
            else
            {
                Debug.Log("Did not Hit");
            }
            yield return new WaitForSeconds(fireRate);
        }

        gunAudioSource.Stop();
        gunAudioSource.PlayOneShot(endSound);
        startedFiring = false;
        shooting = null;

    }

    public void OnFire(InputValue inputValue)
    {
        if (IsOwner)
            firing = inputValue.isPressed;
    }
}