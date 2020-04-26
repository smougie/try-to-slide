using UnityEngine;

public class BarTrigger : MonoBehaviour
{
    // Method responsible for tracking collision with player
    private void OnTriggerEnter(Collider other)
    {
        // if player collides with bar, pulling BarTrigger method from SlidingWall class
        if (other.transform.tag == "Player")
        {
            gameObject.GetComponentInParent<SlidingWall>().BarTrigger();
        }
    }
}
