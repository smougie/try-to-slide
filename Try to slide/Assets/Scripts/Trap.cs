using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private float delayTime = 0;
    [SerializeField] private float repeatRate = 0;
    private Animation spikeAnimation;

    void Start()
    {
        spikeAnimation = gameObject.GetComponent<Animation>();
        InvokeRepeating("ObjectAnimation", delayTime, repeatRate);
    }

    void ObjectAnimation()
    {
        spikeAnimation.Play();
    }
}
