using UnityEngine;
using System.Collections;

public class BladeTrap : MonoBehaviour
{
    [SerializeField] private float delayTime = 0;
    private Animation bladeAnimation;
    private Animation platformAnimation;
    
    void Start()
    {
        bladeAnimation = transform.Find("Blade").GetComponent<Animation>();
        platformAnimation = transform.Find("Platform").GetComponent<Animation>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            platformAnimation.Play();
            StartCoroutine(DelayedAnimation(delayTime));
        }
    }

    IEnumerator DelayedAnimation(float sec)
    {
        yield return new WaitForSeconds(sec);
        bladeAnimation.Play();
    }
}
