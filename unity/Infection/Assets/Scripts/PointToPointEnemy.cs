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
    public override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        Gizmos.DrawSphere(PointA, collisionRadius);
        Gizmos.DrawSphere(PointB, collisionRadius);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void DetermineInput()
    {
        input = Vector2.zero;
        if (!isSimulated && !isRecoiling && !IsAttacking() && isGrounded)
        {
            Vector3 delta = Target - transform.position;
            if (ignoreY)
                delta.y = 0;
            float dist = delta.magnitude;
            if (dist < 0.1f)
                targetingA = !targetingA;
            else
            {
                input = delta.normalized;
            }

        }
        else
            base.DetermineInput();

    }
}

