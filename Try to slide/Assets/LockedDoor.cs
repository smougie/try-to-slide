using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    [SerializeField] private GameObject doors;
    [SerializeField] private GameObject key;

    private Vector3 doorStartingRotation;
    private Vector3 doorEndingRotation;

    private bool openingDoors;
    private int doorCounter = 0;

    private float doorStartingY;
    
    void Start()
    {
        doorStartingRotation = new Vector3(doors.transform.rotation.x, doors.transform.rotation.y, doors.transform.rotation.z);
        doorEndingRotation = new Vector3(doorStartingRotation.x, doorStartingRotation.y - 90, doorStartingRotation.z);
        doorStartingY = doors.transform.rotation.y;
    }

    void Update()
    {
        if (openingDoors)
        {
            doors.transform.Rotate(0, -1, 0);
            doorCounter += 1;
        }
        if (doorCounter >= 91)
        {
            openingDoors = false;
            Destroy(doors);
        }
    }

    public void KeyTrigger()
    {
        Destroy(key);
        openingDoors = true;
    }
}
