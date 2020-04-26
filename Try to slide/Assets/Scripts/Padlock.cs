using UnityEngine;

public class Padlock : MonoBehaviour
{
    // Method responsible for tracking collision with world
    private void OnTriggerStay(Collider other)
    {
        // if padlock is colliding with World tag object (World Node), rotating padlock around World Node
        if (other.transform.tag == "World")
        {
            transform.RotateAround(other.transform.position, Vector3.up, .5f);
        }
    }
}
