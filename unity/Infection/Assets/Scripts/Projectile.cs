using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Projectile : MonoBehaviour
{
    public float speed;
    Rigidbody rBody;
    public void Init(float lifeSpan)
    {
        Destroy(this.gameObject, lifeSpan);
    }
    public void Shoot(Vector3 dir)
    {

    }
}

