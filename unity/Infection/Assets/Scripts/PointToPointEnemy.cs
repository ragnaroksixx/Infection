using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PointToPointEnemy : EnemyMovement
{
    public Vector3 localPointA, localPointB;
    public Vector3 PointA { get => localPointA + room.transform.position; }
    public Vector3 PointB { get => localPointB + room.transform.position; }
    public Vector3 Target { get => targetingA ? PointA : PointB; }

    public bool targetingA;
    public bool ignoreY = true;
    public bool flier;
    public void OnDrawGizmosSelected()
    {
        if (room == null) return;
        Gizmos.DrawSphere(PointA, 0.2f);
        Gizmos.DrawSphere(PointB, 0.2f);
    }
    public override void SetInput(Movement m)
    {
        base.SetInput(m);
        if (!m.IsSimulated && !m.isRecoiling && !m.IsAttacking() && (m.isGrounded || flier))
        {
            Vector3 delta = Target - transform.position;
            if (ignoreY)
                delta.y = 0;
            delta.z = 0;
            float dist = delta.magnitude;
            if (dist < 0.1f)
                targetingA = !targetingA;
            else
            {
                m.input = delta.normalized;
            }

        }
    }

}

