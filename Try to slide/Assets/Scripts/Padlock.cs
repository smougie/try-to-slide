using UnityEngine;

public class Padlock : MonoBehaviour
{

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "World")
        {
            transform.RotateAround(other.transform.position, Vector3.up, .5f);
        }
    }
}
