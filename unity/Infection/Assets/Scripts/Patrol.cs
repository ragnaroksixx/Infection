using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : EnemyMovement
{
    public Transform groundDetection;
    public Transform fwdDetection;
    public LayerMask layer;
    public override void SetInput(Movement m)
    {
        base.SetInput(m);

        if (!m.IsSimulated && !m.isRecoiling && !m.IsAttacking() && (m.isGrounded))
        {
            if (Physics.Raycast(groundDetection.transform.position, Vector2.down, .2f, layer))
            {
                m.FaceDirection(!m.isFacingRight);
            }

            if (Physics.Raycast(fwdDetection.transform.position, transform.right, .2f, layer))
            {
                m.FaceDirection(!m.isFacingRight);
            }

            m.input = m.isFacingRight ? Vector3.right : -Vector3.right;
        }
    }
}

