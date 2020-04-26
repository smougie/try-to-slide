using UnityEngine;

// Class responsible for tracking collision with player, if player collides with swtich pulling method SwitchTrigger from SwitchWall class
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
