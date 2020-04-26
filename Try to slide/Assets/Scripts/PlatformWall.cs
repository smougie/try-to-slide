using UnityEngine;
using System.Collections;

public class PlatformWall : MonoBehaviour
{
    #region Variables

    [SerializeField] private GameObject wall = null;  // wall object
    [SerializeField] private GameObject platform= null;  // platform object

    [SerializeField] private float platformFallSpeed = .25f;  // platform falling down speed
    [SerializeField] private float wallLiftSpeed = 1f;  // wall lifting up speed
    [SerializeField] private float wallLiftDropSpeed = 8f;  // wall dropping speed
    [SerializeField] private float wallLiftingTime = 2f;  // wall lift up stime

    private Vector3 platformStartPosition;  // platform start position
    private Vector3 platformEndPosition;  // platform end position
    private Vector3 wallStartPosition;  // wall start position
    private Vector3 wallEndPosition;  // wall end position

    private bool mechanismWorking;  // flag for working wall mechanism

    #endregion

    void Start()
    {
        platformStartPosition = platform.transform.position;  // initializng variable with wall starting position
        platformEndPosition = new Vector3(platformStartPosition.x, -.6f, platformStartPosition.z);  // initializng variable with wall end position

        wallStartPosition = wall.transform.position;  // initializing wall start position
        wallEndPosition = new Vector3(wallStartPosition.x, wallStartPosition.y + 1.5f, wallStartPosition.z);  // initializing wall end position
    }

    void Update()
    {
        // if flag is raised moving wall to wall end position and platform to end position
        if (mechanismWorking)
        {
            wall.transform.position = Vector3.MoveTowards(wall.transform.position, wallEndPosition, wallLiftSpeed * Time.deltaTime);
            platform.transform.position = Vector3.MoveTowards(platform.transform.position, platformEndPosition, platformFallSpeed * Time.deltaTime);
        }
        // if flag is dropped moving wall and platform to start position
        if (!mechanismWorking)
        {
            wall.transform.position = Vector3.MoveTowards(wall.transform.position, wallStartPosition, wallLiftDropSpeed * Time.deltaTime);
            platform.transform.position = Vector3.MoveTowards(platform.transform.position, platformStartPosition, wallLiftingTime * Time.deltaTime);
        }
        // if mechanism is not working (dropping wall and platform moving to start position) and wall is not in start position, so wall is dropping
        // down site of wall is deadly for player, collider is triggered as physical collider
        if (!mechanismWorking && wall.transform.position != wallStartPosition)
        {
            wall.transform.tag = "Physical";
            wall.GetComponent<BoxCollider>().isTrigger = true;
        }
        else
        {
            wall.transform.tag = "Untagged";
            wall.GetComponent<BoxCollider>().isTrigger = false;
        }
    }

    // Method responsible for setting mechanismWorking flag to true and if false starting coroutine which is delaying wall drop time
    public void isActive(bool isActive)
    {
        if (isActive)
        {
            mechanismWorking = true;
        }
        else
        {
            StartCoroutine("MechanismDelay", wallLiftingTime);
        }
    }

    private IEnumerator MechanismDelay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        mechanismWorking = false;
    }
}
