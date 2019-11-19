using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;


public class Movement : MonoBehaviour
{
    public enum State
    {
        NORMAL,
        RECOIL,
        SIMULATED,
    }
    public float speed = 5;
    public float jumpSpeed = 5;
    protected Vector2 input;
    Vector2 simulatedInput;
    protected Rigidbody rBody;

    public float fallMult = 2.5f;

    public float lowJumpMult = 2.0f;

    public bool isGrounded = false;
    public bool wasGrounded;
    public LayerMask groundLayer;
    public float collisionRadius = 1;
    public Transform bottomOffset;
    public bool isFacingRight = true;
    public float jumpHoldTime = 1;
    protected float jumpHoldTimeTrack;
    bool holdingJump = true;

    public float coyoteTime = 0.1f;
    public float coyoteTimeTrack;
    float recoilTrack;
    public bool isRecoiling;
    protected bool isSimulated;
    Vector2 recoilDir;
    public float drag = 1;
    public virtual void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        OnUpdate();
    }
    public virtual void DetermineInput()
    {
        input = Vector2.zero;
        if (isRecoiling)
            input = recoilDir;
        else
        {
            if (isSimulated)
            {
                input = simulatedInput;
            }
            else
            {

            }
        }
    }
    public virtual void OnUpdate()
    {
        wasGrounded = isGrounded;
        isGrounded = Physics.OverlapSphere(bottomOffset.position, collisionRadius, groundLayer).Length > 0;
        if (wasGrounded && !isGrounded)
        {
            OnGrounded();
        }

        DetermineInput();

        if (isRecoiling)
        {
            if (Time.time > recoilTrack) isRecoiling = false;
            SetVelocity(input, 1, false);
        }
        else
        {
            if (input.x > 0 && !isFacingRight)
                FaceDirection(true);
            else if (input.x < 0 && isFacingRight)
                FaceDirection(false);
            SetVelocity(input, speed, true);
        }


        if (!isGrounded)
        {
            holdingJump = IsHoldingJump();
            jumpHoldTimeTrack -= Time.deltaTime;

            if (rBody.velocity.y < 0)
            {
                OnFallingDown();
            }
            else if (rBody.velocity.y > 0 && !holdingJump)
            {
                jumpHoldTimeTrack -= Time.deltaTime;
                rBody.velocity += Vector3.up * Physics.gravity.y * (lowJumpMult - 1) * Time.deltaTime;
            }
        }

        if (ShouldJump())
            Jump();
    }
    public virtual bool IsHoldingJump()
    {
        return false;
    }

    public virtual bool ShouldJump()
    {
        return false;
    }
    public virtual void OnFallingDown()
    {
        rBody.velocity += Vector3.up * Physics.gravity.y * (fallMult - 1) * Time.deltaTime;
    }
    public virtual void OnGrounded()
    {
        coyoteTimeTrack = Time.time + coyoteTime;
    }

    public void SetVelocity(Vector2 i, float spd, bool ignoreY)
    {
        i *= spd;

        if (i == Vector2.zero)
        {
            Vector3 targetVelocity = Vector2.zero;


            targetVelocity = Vector3.Slerp(rBody.velocity, targetVelocity, Time.deltaTime * drag);
            //if (!ignoreY)
            targetVelocity.y = rBody.velocity.y;
            rBody.velocity = targetVelocity;
        }
        else
        {
            if (ignoreY)
                rBody.velocity = new Vector2(i.x, rBody.velocity.y);
            else
                rBody.velocity = i;
        }
    }

    void FaceDirection(bool right)
    {
        isFacingRight = right;
        transform.localEulerAngles = new Vector3(0, right ? 0 : 180, 0);
    }

    public virtual void Jump()
    {
        jumpHoldTimeTrack = jumpHoldTime;
        rBody.velocity = new Vector2(rBody.velocity.x, 0);
        rBody.velocity += Vector3.up * jumpSpeed;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(bottomOffset.position, collisionRadius);
    }

    public virtual void HitCharacter(Vector3 dir, float stunTime)
    {
        isRecoiling = true;
        recoilDir = dir;
        recoilTrack = Time.time + stunTime;
    }

    public void SimulateInput(Vector2 i)
    {
        isSimulated = true;
        simulatedInput = i;
    }

    public void StopSimulateInput()
    {
        isSimulated = false;
    }

    public virtual bool IsAttacking()
    {
        return false;
    }
}

