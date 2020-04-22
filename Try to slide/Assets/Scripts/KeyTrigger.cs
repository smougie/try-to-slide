using UnityEngine;

public class KeyTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            gameObject.GetComponentInParent<LockedDoor>().KeyTrigger();
        }
    }
}
