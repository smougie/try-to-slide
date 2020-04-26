using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class responsible for moving object up and down
public class MoveUpAndDown : MonoBehaviour
{
    #region Variables

    public float moveSpeed;  // object moving speed
    public float height;  // variable storing value of moving up
    private Vector3 spawnPosition;  // spawn position
    private Vector3 upPosition;  // maximum up postion
    private Vector3 downPosition;  // lowest position
    private Vector3 movingTo;  // position of object which currently moving to

    #endregion

    void Start()
    {
        downPosition = transform.position;  // initializing starting position
        upPosition = new Vector3(transform.position.x, transform.position.y + height, transform.position.z);  // counting Vector3 position for up position
    }

    void Update()
    {
        // if transform position is equal to down position setting movingTo position for up position
        if (transform.position == downPosition)
        {
            movingTo = upPosition;
        }

        // if transform position is equal to up position, setting movingTo position for down position
        if (transform.position == upPosition)
        {
            movingTo = downPosition;
        }

        // syntax responsible for moving object
        transform.position = Vector3.MoveTowards(transform.position, movingTo, moveSpeed * Time.deltaTime);
    }
}
