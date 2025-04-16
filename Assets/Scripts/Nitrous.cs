using System;
using System.Collections;
using System.Globalization;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Nitrous : NetworkBehaviour
{
    public AudioSource nosAudio;
    public AudioClip nosStart;
    public ParticleSystem nosPS1, nosPS2;
    private Rigidbody rb;
    private PrometeoCarController carController;
    // NOS Bar
    private Slider boostTimeSlider;

    // Used to start and stop the turret firing
    bool canBoost = false;
    bool boosting = false;
    bool startedBoosting = false;
    float boostMaxTime = 5f;
    [Range(0f, 5f)]
    float boostTime = 0;
    float coolDown = 0;
    int boostPower = 50;
    int defaultAcceleration;
    int defaultMaxSpeed;
    int boostSpeed = 600;

    Coroutine boost = null;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (IsOwner)
        {
            boostTimeSlider = GameObject.FindWithTag("BoostTime").GetComponent<Slider>();
            carController = GetComponent<PrometeoCarController>();
            rb = GetComponent<Rigidbody>();
            defaultAcceleration = carController.accelerationMultiplier;
            boostTimeSlider.value = boostMaxTime;
            defaultMaxSpeed = carController.maxSpeed;
        }

        canBoost = true;
        coolDown = 0;
        boostTime = boostMaxTime;
    }

    // Update is called once per frame
    void Update()
    {
        HandleCanBoost();
        if (IsOwner)
        {
            Boost();
        }
    }

    public void Boost()
    {
        if (IsOwner)
            boostTimeSlider.value = boostTime;

        // if can fire turret activates
        if (canBoost && boosting && boostTime > 0) // firing 
        {
            if(startedBoosting == false)
            {
                nosAudio.PlayOneShot(nosStart);
                startedBoosting = true;
                nosAudio.PlayDelayed(2.5f);
                nosPS1.Play();
                nosPS2.Play();
            }

            boostTime -= Time.deltaTime;

            if (IsOwner)
            {
                rb.AddForce(transform.forward * boostPower, ForceMode.Impulse);
            }
                
        }
        else
        {

        }

        if (coolDown <= 0 && boosting == false && boostTime < boostMaxTime) // increase fire time if not firing and not in cooldown
        {
            boostTime += Time.deltaTime/4;
            nosAudio.Stop();
            nosPS1.Stop();
            nosPS2.Stop();
            startedBoosting = false;
        }

        if (boostTime > boostMaxTime) boostTime = boostMaxTime; // set fire time to ma



        /*if (boosting && canBoost && boostTime > 0 && carController.accelerationMultiplier != boostPower)
        {
            carController.maxSpeed = boostSpeed;
            carController.accelerationMultiplier = boostPower; // boosting
        }

        if ((boosting == false || canBoost == false) && carController.accelerationMultiplier != defaultAcceleration) // firing 
        {
            carController.maxSpeed = defaultMaxSpeed;
            carController.accelerationMultiplier = defaultAcceleration;
            // Start Audio Source
            //gunAudioSource.Play();
        }*/
    }

    void HandleCanBoost()
    {
        if (boostTime <= 0 && coolDown <= 0) // reset cooldown
        {
            coolDown = 5f;
        }

        if (coolDown > 0) // decrease cooldown time
        {
            if (canBoost != false)
                canBoost = false;
            if (boosting != false)
                boosting = false;
            if (startedBoosting == true)
                startedBoosting = false;
            if(nosAudio.isPlaying)
                nosAudio.Stop();
            if (nosPS1.isPlaying)
                nosPS1.Stop();
            if (nosPS2.isPlaying)
                nosPS2.Stop();
            coolDown -= Time.deltaTime;
            if (coolDown <= 0) boostTime = 0.01f;
        }

        if (coolDown <= 0 && boostTime > 0) // if cooldown ended and fire time is greater than 0, then canFire
        {
            canBoost = true;
        }


    }

    public void OnBoost(InputValue inputValue)
    {
        boosting = inputValue.isPressed;
    }
}
