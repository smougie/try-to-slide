using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 0f;
    [SerializeField] private int direction = 0;

    private void Update()
    {
        transform.RotateAround(transform.position, new Vector3(0, direction, 0), (rotateSpeed * 10) * Time.deltaTime);
    }
}
