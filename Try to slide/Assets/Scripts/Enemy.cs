using UnityEngine;

// Enemy class which is responsible for controling the enemy unit, which will patrol map areas created using patrol points.
public class Enemy : MonoBehaviour
{
    public Transform[] patrolPoints;  // patrol points, create patrol point in Unity then attach to patrol unit
    [SerializeField] public float moveSpeed;  // enemy movement speed accesible from Unity inspector
    private int currentPoint = 0;  // variable created to store patrol point number for moving enemy unit toward it


    void Start()
    { 
        transform.position = patrolPoints[0].position;  // at start position of enemy player is set to first patrol point in array
    }


    void Update()
    {
        // if current position of enemy is equal to spawn point (enemy reached spawn point), change for next patrol point in array
        // and force enemy unit to move to that patrol point
        if (transform.position == patrolPoints[currentPoint].position)
        {
            currentPoint++;
        }

        // statement responsible for tracking last spawn point, if patrol point is equal to last patrol point
        // set patrol point to first one
        if (currentPoint == patrolPoints.Length)
        {
            currentPoint = 0;
        }

        // moving enemy unit - move towards currentPoint(current position of unit, move to this patrol point, velocity)
        transform.position = Vector3.MoveTowards(transform.position, patrolPoints[currentPoint].position, moveSpeed * Time.deltaTime);
    }
}
