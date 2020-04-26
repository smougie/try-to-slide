using UnityEngine;
using System.Collections;

public class BladeTrap : MonoBehaviour
{
    #region Variables

    [SerializeField] private float delayTime = 0;
    private Animation bladeAnimation;
    private Animation platformAnimation;

    #endregion

    // Initializing blade and platform variables storing their animations
    void Start()
    {
        bladeAnimation = transform.Find("Blade").GetComponent<Animation>();
        platformAnimation = transform.Find("Platform").GetComponent<Animation>();
    }

    // Method responsible for tracking player collision
    // if platform collides with player object playing platform animation and starting coroutine for blade animation
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
