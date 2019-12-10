using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    Rigidbody rBody;

    public Rigidbody RBody { get => rBody; set => rBody = value; }

    public void Init(float lifeSpan)
    {
        rBody = GetComponent<Rigidbody>();
        Destroy(this.gameObject, lifeSpan);
    }
    public void Shoot(Vector3 dir)
    {

    }
}

