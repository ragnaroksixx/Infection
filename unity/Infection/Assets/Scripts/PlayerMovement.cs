using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class PlayerMovement : Movement
{
    public static PlayerMovement instance;
    public float fastFallMult = 2.5f;
    bool fastFall;
    public Attack meleeAttack;
    public Attack ariealAttack;
    public ClawAttack clawAttack;
    public int maxJumps = 1;
    int jumpTrack = 0;
    private void Awake()
    {
        instance = this;
    }
    public override void OnGrounded()
    {
        base.OnGrounded();
        fastFall = false;
        jumpTrack = maxJumps;
    }
    public override void DetermineInput()
    {
        input = Vector2.zero;
        if (!isSimulated && !isRecoiling && !IsAttacking())
        {
            if (Input.GetKey(KeyCode.A))
            {
                input.x -= 1;
            }
            if (Input.GetKey(KeyCode.D))
            {
                input.x += 1;
            }
        }
        else
            base.DetermineInput();
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        if (!IsAttacking())
        {
            if (isGrounded && Input.GetKeyDown(meleeAttack.key))
            {
                meleeAttack.StartAttack();
            }
            else if (!isGrounded && Input.GetKeyDown(ariealAttack.key))
            {
                ariealAttack.StartAttack();
            }
            else if (Input.GetKeyDown(clawAttack.key))
            {
                clawAttack.StartAttack();
            }
            else if ((isGrounded || Time.time < coyoteTimeTrack) && Input.GetKeyDown(KeyCode.Space))
                Jump();
        }
    }
    public override bool ShouldJump()
    {
        bool canJump = jumpTrack > 0 || Time.time < coyoteTimeTrack;
        canJump = canJump && Input.GetKeyDown(KeyCode.Space);
        canJump = canJump && !IsAttacking() && !isRecoiling;
        return canJump;
    }
    public override bool IsHoldingJump()
    {
        return jumpHoldTimeTrack > 0 && Input.GetKey(KeyCode.Space);
    }
    public override void OnFallingDown()
    {
        if (Input.GetKey(KeyCode.S))
            fastFall = true;
        if (fastFall)
            rBody.velocity += Vector3.up * Physics.gravity.y * (fastFallMult - 1) * Time.deltaTime;
        else
            base.OnFallingDown();
    }

    public override bool IsAttacking()
    {
        return meleeAttack.IsAttacking || ariealAttack.IsAttacking || clawAttack.IsAttacking;
    }
    public override void Jump()
    {
        base.Jump();
        jumpTrack--;
    }
}
