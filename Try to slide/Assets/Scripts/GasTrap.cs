using UnityEngine;
using System.Collections;

public class GasTrap : MonoBehaviour
{
    #region Variables
    
    // Time variables
    [SerializeField] private float delayTime = 0f;  // trap activation will be delayed by this value
    [SerializeField] private float pauseTime = 3f;  // pause time between gas animation and box collider activation
    [SerializeField] private float gasDuration = 1f;  // duration of gas animation

    // Particles 
    [SerializeField] private ParticleSystem gas1 = null;
    [SerializeField] private ParticleSystem gas2 = null;
    [SerializeField] private ParticleSystem gas3 = null;
    [SerializeField] private ParticleSystem gas4 = null;

    // Booleans and counter
    [SerializeField] private bool loop = false;
    private bool trapNotLooped = false;  // flag necessary for trap coroutine while loop
    private bool ColliderActive = false;
    private float boxColliderCounter;

    #endregion

    // Start method in which the IEnumerator was used for delaying trap activation by delayTime accesible from inspector
    IEnumerator Start()
    {
        // Setting gas duration or gas loop
        SetGasDuration(loop);

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
            trapMechanismLooped();  // activating looped gas mechanism
        }
    }

    private void Update()
    {
        ColliderStatus();  // tracking collider status if flag is raised collider is enabled, when flag is dropped collider disabled
        // condition tracking for trap type, trap can be set for time (break between gas production) or looped (continuous gas production)
        if (trapNotLooped)
        {
            // if trap is not looped checking conditions, implemented base counter for gas animation which is synchonized with box collider
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
            ActivateGas();
            BoxColliderOn();
            yield return new WaitForSeconds(pauseTime);
        }
    }

    // Method responsible for tracking collider status, if flag is raised/gas flies - collider is enabled otherwise flag is dropped
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

    // Setting box collider counter, gas animation + gas flies animation
    private void BoxColliderOn()
    {
        boxColliderCounter = gasDuration + .5f;
    }

    // Activating gas animation
    private void ActivateGas()
    {
        gas1.Play();  
        gas2.Play();  
        gas3.Play();  
        gas4.Play();  
    }

    // Setting Gas duration, if passing loop = true changing loop gas for looping mode, if loop is false setting gas duration accesible from inspector
    private void SetGasDuration(bool loop)
    {
        var gasMain1 = gas1.main;
        var gasMain2 = gas2.main;
        var gasMain3 = gas3.main;
        var gasMain4 = gas4.main;

        if (!loop)
        {
            gasMain1.duration = gasDuration;
            gasMain2.duration = gasDuration;
            gasMain3.duration = gasDuration;
            gasMain4.duration = gasDuration;
        }
        else
        {
            gasMain1.loop = true;
            gasMain2.loop = true;
            gasMain3.loop = true;
            gasMain4.loop = true;
        }
    }

    // Method responsible for raising collider flag which and activating gas animation, both are looped and enabled
    private void trapMechanismLooped()
    {
        ColliderActive = true;
        ActivateGas();
    }
}
