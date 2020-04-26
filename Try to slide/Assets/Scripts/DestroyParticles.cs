using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticles : MonoBehaviour
{
    public float lifetime;

    // After instantiate particles object, destroying them after lifetime time
    void Start()
    {
        Destroy(gameObject, lifetime);
    }
}