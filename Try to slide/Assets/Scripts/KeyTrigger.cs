using UnityEngine;

public class KeyTrigger : MonoBehaviour
{

    // Method responsible for tracking collisions with player object
    private void OnTriggerEnter(Collider other)
    {
        // if player trigger key, pulling KeyTrigger method from LockedDoor class
        if (other.transform.tag == "Player")
        {
            gameObject.GetComponentInParent<LockedDoor>().KeyTrigger();
        }
    }
}
