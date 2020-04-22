using UnityEngine;

public class PlatformTrigger : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            gameObject.GetComponentInParent<PlatformWall>().isActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        gameObject.GetComponentInParent<PlatformWall>().isActive(false);
    }
}
