using UnityEngine;

public class BarTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            gameObject.GetComponentInParent<SlidingWall>().BarTrigger();
        }
    }
}
