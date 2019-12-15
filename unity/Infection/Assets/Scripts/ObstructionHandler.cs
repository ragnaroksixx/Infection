using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstructionHandler : MonoBehaviour
{
    public Vector3 size;
    public LayerMask layer;
    public bool IsBlocked()
    {
        Collider[] col = Physics.OverlapBox(transform.position, size / 2, Quaternion.identity, layer);
        return col.Length > 0;
    }
}

