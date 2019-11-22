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
    public void OnDrawGizmosSelected()
    {
        if (movement == null) return;
        Gizmos.DrawSphere(PointA, movement.CollisionRadius);
        Gizmos.DrawSphere(PointB, movement.CollisionRadius);
    }
    public override void SetInput()
    {
        base.SetInput();
        if (!movement.IsSimulated && !movement.isRecoiling && !movement.IsAttacking() && movement.isGrounded)
        {
            Vector3 delta = Target - transform.position;
            if (ignoreY)
                delta.y = 0;
            float dist = delta.magnitude;
            if (dist < 0.1f)
                targetingA = !targetingA;
            else
            {
                movement.input = delta.normalized;
            }

        }
    }

}

