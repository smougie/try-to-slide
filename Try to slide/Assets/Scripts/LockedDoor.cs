using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    #region Variables

    [SerializeField] private GameObject doors = null;  // variable storing door game object
    [SerializeField] private GameObject key = null;  // variable storing key game object

    private Vector3 doorStartingRotation;  // variable storing door starting rotation
    private Vector3 doorEndingRotation;  // variable storing door ending rotation

    private bool openingDoors;  // variable storing flag raised when script is starting opening the doors
    private int doorCounter = 0;  // variable storing angle progress while rotating doors

    private float doorStartingY;  // variable storing door starting Y rotation 
    
    #endregion

    void Start()
    {
        doorStartingRotation = new Vector3(doors.transform.rotation.x, doors.transform.rotation.y, doors.transform.rotation.z);
        doorEndingRotation = new Vector3(doorStartingRotation.x, doorStartingRotation.y - 90, doorStartingRotation.z);
        doorStartingY = doors.transform.rotation.y;
    }

    void Update()
    {
        // simple counter, which is rotating doors on Y rotation, when doors move 90 degrees, counter stop moving doors
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

    // Method responsible for start opening doors and destroying key object
    public void KeyTrigger()
    {
        Destroy(key);
        openingDoors = true;
    }
}
