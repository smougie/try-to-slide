using UnityEngine;
using System.Collections;

public class IceTrap : MonoBehaviour
{
    #region Variables

    // Time variables
    [SerializeField] private float delayTime = 0f;  // trap activation will be delayed by this value
    [SerializeField] private float pauseTime = 3f;  // pause time between ice animation and box collider activation
    [SerializeField] private float iceDuration = 1f;  // duration of ice animation

    // Particles 
    [SerializeField] private ParticleSystem ice1 = null;
    [SerializeField] private ParticleSystem ice2 = null;

    // Booleans and counter
    [SerializeField] private bool loop = false;
    private bool trapNotLooped = false;  // flag necessary for trap coroutine while loop
    private bool ColliderActive = false;
    private float boxColliderCounter;

    #endregion

    // Start method in which the IEnumerator was used for delaying trap activation by delayTime accesible from inspector
    IEnumerator Start()
    {
        // Setting ice duration or ice loop
        SetIceDuration(loop);

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
            trapMechanismLooped();  // activating looped ice mechanism
        }
    }

    private void Update()
    {
        ColliderStatus();  // tracking collider status if flag is raised collider is enabled, when flag is dropped collider disabled
        // condition tracking for trap type, trap can be set for time (break between ice production) or looped (continuous ice production)
        if (trapNotLooped)
        {
            // if trap is not looped checking conditions, implemented base counter for ice animation which is synchonized with box collider
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
            ActivateIce();
            BoxColliderOn();
            yield return new WaitForSeconds(pauseTime);
        }
    }

    // Method responsible for tracking collider status, if flag is raised/ice flies - collider is enabled otherwise flag is dropped
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

    // Setting box collider counter, ice animation + ice flies animation
    private void BoxColliderOn()
    {
        boxColliderCounter = iceDuration + .6f;
    }

    // Activating ice animation
    private void ActivateIce()
    {
        ice1.Play();
        ice2.Play();
    }

    // Setting ice duration, if passing loop = true changing loop ice for looping mode, if loop is false setting ice duration accesible from inspector
    private void SetIceDuration(bool loop)
    {
        var iceMain1 = ice1.main;
        var iceMain2 = ice2.main;


        if (!loop)
        {
            iceMain1.duration = iceDuration;
            iceMain2.duration = iceDuration;
        }
        else
        {
            iceMain1.loop = true;
            iceMain2.loop = true;
        }
    }

    // Method responsible for raising collider flag which and activating ice animation, both are looped and enabled
    private void trapMechanismLooped()
    {
        ColliderActive = true;
        ActivateIce();
    }
}
