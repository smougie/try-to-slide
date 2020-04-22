using UnityEngine;
using System.Collections;

public class PlatformWall : MonoBehaviour
{
    [SerializeField] private GameObject wall = null;
    [SerializeField] private GameObject platform= null;

    [SerializeField] private float platformFallSpeed = .25f;
    [SerializeField] private float wallLiftSpeed = 1f;
    [SerializeField] private float wallLiftDropSpeed = 8f;
    [SerializeField] private float wallLiftingTime = 2f;

    private Vector3 platformStartPosition;
    private Vector3 platformEndPosition;
    private Vector3 wallStartPosition;
    private Vector3 wallEndPosition;

    private bool mechanismWorking;

    
    void Start()
    {
        platformStartPosition = platform.transform.position;
        platformEndPosition = new Vector3(platformStartPosition.x, -.6f, platformStartPosition.z);

        wallStartPosition = wall.transform.position;
        wallEndPosition = new Vector3(wallStartPosition.x, wallStartPosition.y + 1.5f, wallStartPosition.z);
    }

    void Update()
    {
        if (mechanismWorking)
        {
            wall.transform.position = Vector3.MoveTowards(wall.transform.position, wallEndPosition, wallLiftSpeed * Time.deltaTime);
            platform.transform.position = Vector3.MoveTowards(platform.transform.position, platformEndPosition, platformFallSpeed * Time.deltaTime);
        }
        if (!mechanismWorking)
        {
            wall.transform.position = Vector3.MoveTowards(wall.transform.position, wallStartPosition, wallLiftDropSpeed * Time.deltaTime);
            platform.transform.position = Vector3.MoveTowards(platform.transform.position, platformStartPosition, wallLiftingTime * Time.deltaTime);
        }
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
