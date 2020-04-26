using UnityEngine;

// Class responsible for trakcing platfom collisions with player
public class PlatformTrigger : MonoBehaviour
{
    // Method responsible for tracking collisions with player
    private void OnTriggerStay(Collider other)
    {
        // if colliding with player, pulling isActive method with true as parameter
        if (other.transform.tag == "Player")
        {
            gameObject.GetComponentInParent<PlatformWall>().isActive(true);
        }
    }

    // Method responsible for tracking when player leave collider, pulling isActive method with false as parameter
    private void OnTriggerExit(Collider other)
    {
        gameObject.GetComponentInParent<PlatformWall>().isActive(false);
    }
}
