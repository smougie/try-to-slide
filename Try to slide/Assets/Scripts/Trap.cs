using UnityEngine;

// Trap class responsible for playing trap object
public class Trap : MonoBehaviour
{
    #region Variables

    [SerializeField] private float delayTime = 0;  // trap delay time
    [SerializeField] private float repeatRate = 0;  // trap repeat rate in seconds
    private Animation spikeAnimation;  // variable storing spike animation

    #endregion

    void Start()
    {
        spikeAnimation = gameObject.GetComponent<Animation>();  // initializng spike animation
        InvokeRepeating("ObjectAnimation", delayTime, repeatRate);  // starting invoke repeating of spike animation
    }

    // Method responsible for playing spike animation
    void ObjectAnimation()
    {
        spikeAnimation.Play();
    }
}
