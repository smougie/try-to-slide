using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUpAndDown : MonoBehaviour
{
    public float moveSpeed;
    public float height;
    private Vector3 spawnPosition;
    private Vector3 upPosition;
    private Vector3 downPosition;
    private Vector3 movingTo;

    void Start()
    {
        downPosition = transform.position;
        upPosition = new Vector3(transform.position.x, transform.position.y + height, transform.position.z);
    }

   
    void Update()
    {
        if (transform.position == downPosition)
        {
            movingTo = upPosition;
        }

        if (transform.position == upPosition)
        {
            movingTo = downPosition;
        }

        transform.position = Vector3.MoveTowards(transform.position, movingTo, moveSpeed * Time.deltaTime);
    }
}
