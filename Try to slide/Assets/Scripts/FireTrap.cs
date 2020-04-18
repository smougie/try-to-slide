using UnityEngine;
using System.Collections;

public class FireTrap : MonoBehaviour
{
    #region Variables

    // Time variables
    [SerializeField] private float delayTime = 0f;  // trap activation will be delayed by this value
    [SerializeField] private float pauseTime = 3f;  // pause time between fire animation and box collider activation
    [SerializeField] private float fireDuration = 1f;  // duration of fire animation

    // Particles 
    [SerializeField] private ParticleSystem fire = null;

    // Booleans and counter
    [SerializeField] private bool loop = false;
    private bool trapNotLooped = false;  // flag necessary for trap coroutine while loop
    private bool ColliderActive = false;
    private float boxColliderCounter;

    #endregion

    // Start method in which the IEnumerator was used for delaying trap activation by delayTime accesible from inspector
    IEnumerator Start()
    {
        // Setting fire duration or fire loop
        SetFireDuration(loop);

        // if loop is not selected in inspector
        if (!loop)
        {
            yield return new WaitForSeconds(delayTime);  // delaying trap start
            trapNotLooped = true;  // setting not looped flag
            StartCoroutine(trapMechanism());
        }
        // if loop is selected in inspector
        else
        {
            trapNotLooped = false;  // making sure that flag will be droped when game starts
            yield return new WaitForSeconds(delayTime);  //  delaying trap start
            trapMechanismLooped();  // activating looped fire mechanism
        }
    }

    private void Update()
    {
        ColliderStatus();  // tracking collider status if flag is raised collider is enabled, when flag is dropped collider disabled
        // condition tracking for trap type, trap can be set for time (break between fire production) or looped (continuous fire production)
        if (trapNotLooped)
        {
            // if trap is not looped checking conditions, implemented base counter for fire animation which is synchonized with box collider
            if (boxColliderCounter > 0)
            {
                boxColliderCounter -= Time.deltaTime;
                ColliderActive = true;
            }
            if (boxColliderCounter <= 0)
            {
                ColliderActive = false;
                boxColliderCounter = 0;
            }
        }
    }

    // Enumerator necessary for looping trap mechanism (animation, box collider and breaks between)
    private IEnumerator trapMechanism()
    {
        while (trapNotLooped)
        {
            ActivateFire();
            BoxColliderOn();
            yield return new WaitForSeconds(pauseTime);
        }
    }

    // Method responsible for tracking collider status, if flag is raised/fire flies - collider is enabled otherwise flag is dropped
    private void ColliderStatus()
    {
        if (ColliderActive)
        {
            gameObject.GetComponent<BoxCollider>().enabled = true;
        }
        else
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }

    // Setting box collider counter, fire animation + fire flies animation
    private void BoxColliderOn()
    {
        boxColliderCounter = fireDuration + .1f;
    }

    // Activating fire animation
    private void ActivateFire()
    {
        fire.Play();
    }

    // Setting fire duration, if passing loop = true changing loop fire for looping mode, if loop is false setting fire duration accesible from inspector
    private void SetFireDuration(bool loop)
    {
        var fireMain1 = fire.main;


        if (!loop)
        {
            fireMain1.duration = fireDuration;
        }
        else
        {
            fireMain1.loop = true;
        }
    }

    // Method responsible for raising collider flag which and activating fire animation, both are looped and enabled
    private void trapMechanismLooped()
    {
        ColliderActive = true;
        ActivateFire();
    }
}
