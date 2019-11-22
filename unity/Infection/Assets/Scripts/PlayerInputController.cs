using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerInputController : InputController
{
    public override void SetInput()
    {
        base.SetInput();
        if (!movement.IsSimulated && !movement.isRecoiling && !movement.IsAttacking())
        {
            if (Input.GetKey(KeyCode.A))
            {
                movement.input.x -= 1;
            }
            if (Input.GetKey(KeyCode.D))
            {
                movement.input.x += 1;
            }
        }
    }
    public override void SetAttacks()
    {
        base.SetAttacks();
        if (!movement.IsAttacking())
        {
            foreach (Attack attack in attacks)
            {
                if ((attack.isGroundAttack && movement.isGrounded || attack.isAirAttack && !movement.isGrounded) &&
                    Input.GetKeyDown(attack.key))
                {
                    attack.StartAttack();
                    break;
                }
            }
        }
    }
    public override bool IsHoldingJump()
    {
        return Input.GetKey(KeyCode.Space);
    }

    public override bool IsFastFall()
    {
        return Input.GetKey(KeyCode.S);
    }

    public override bool Jump()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }
}
