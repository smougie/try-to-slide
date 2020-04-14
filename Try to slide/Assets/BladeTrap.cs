using UnityEngine;
using System.Collections;

public class BladeTrap : MonoBehaviour
{
    private Animation bladeAnimation;
    
    void Start()
    {
        bladeAnimation = transform.Find("Blade").GetComponent<Animation>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            StartCoroutine(DelayedAnimation());
        }
    }

    IEnumerator DelayedAnimation()
    {
        yield return new WaitForSeconds(1);
        bladeAnimation.Play();
    }
}
