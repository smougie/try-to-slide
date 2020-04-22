using UnityEngine;

public class SwitchTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            gameObject.GetComponentInParent<SwitchWall>().SwitchTrigger();
        }
    }
}
