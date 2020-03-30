using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField]
    private float delayTime = 0;
    [SerializeField]
    private float pasueTime = 0;
    private Animation spikeAnimation;

    void Start()
    {
        spikeAnimation = gameObject.GetComponent<Animation>();
        InvokeRepeating("ObjectAnimation", delayTime, pasueTime);
    }

    void ObjectAnimation()
    {
        spikeAnimation.Play();
    }
}
