﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5;
    public float airControlSpeed;
    public float jumpSpeed = 5;
    Vector2 input;
    Vector2 simulatedInput;
    Rigidbody rBody;

    public float fallMult = 2.5f;
    public float lowJumpMult = 2.0f;

    public bool isGrounded = false;
    public bool wasGrounded;
    public LayerMask groundLayer;
    public float collisionRadius = 1;
    public Transform bottomOffset;
    public bool isFacingRight = true;
    public float jumpHoldTime = 1;
    float jumpHoldTimeTrack;
    bool holdingJump = true;

    public float coyoteTime = 0.1f;
    public float coyoteTimeTrack;
    public static PlayerMovement instance;

    public float recoilTime = 0.5f;
    float recoilTrack;
    bool isRecoiling;
    bool isSimulated;
    Vector2 recoilDir;

    //public Animator anim;
    //public ParticleSystem dust;
    public float timeScale = 1;
    public float TimeScale { get => Time.deltaTime * timeScale; }

    public bool canMove = true;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        wasGrounded = isGrounded;
        isGrounded = Physics.OverlapSphere(bottomOffset.position, collisionRadius, groundLayer).Length > 0;
        //anim.SetBool("isGrounded", isGrounded);
        if (wasGrounded && !isGrounded)
        {
            coyoteTimeTrack = Time.time + coyoteTime;
        }

        input = Vector2.zero;

        if (isRecoiling)
        {
            input = recoilDir;
            Recoil(input);
            if (Time.time > recoilTrack) isRecoiling = false;
        }
        else
        {
            if (isSimulated)
            {
                input = simulatedInput;
            }
            else if (canMove)
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
            if (input.x > 0 && !isFacingRight)
                FaceDirection(true);
            else if (input.x < 0 && isFacingRight)
                FaceDirection(false);


            Walk(input);

        }

        if (!isGrounded)
        {
            holdingJump = jumpHoldTimeTrack > 0 && Input.GetKey(KeyCode.Space);
            jumpHoldTimeTrack -= TimeScale;

            if (rBody.velocity.y < 0)
            {
                rBody.velocity += Vector3.up * Physics.gravity.y * (fallMult - 1) * TimeScale;
            }
            else if (rBody.velocity.y > 0 && !holdingJump)
            {
                jumpHoldTimeTrack -= TimeScale;
                rBody.velocity += Vector3.up * Physics.gravity.y * (lowJumpMult - 1) * TimeScale;
            }
        }
        if ((isGrounded || Time.time < coyoteTimeTrack) && Input.GetKeyDown(KeyCode.Space))
            Jump();
        //anim.SetFloat("velocityY", rBody.velocity.y);
        SetDustEmmision();
    }
    public void Walk(Vector2 i)
    {
        //anim.SetBool("Walk", (i.x != 0));
        i *= speed;
        rBody.velocity = new Vector2(i.x, rBody.velocity.y);
    }
    public void Recoil(Vector2 i)
    {
        i *= speed;
        rBody.velocity = new Vector2(i.x, 0);
    }
    void FaceDirection(bool right)
    {
        isFacingRight = right;
        transform.localEulerAngles = new Vector3(0, right ? 0 : 180, 0);
    }
    void SetDustEmmision()
    {
        //EmissionModule em = dust.emission;
        //em.enabled = isGrounded && Mathf.Abs(input.x) > 0;
    }

    public void Jump()
    {
        jumpHoldTimeTrack = jumpHoldTime;
        rBody.velocity = new Vector2(rBody.velocity.x, 0);
        rBody.velocity += Vector3.up * jumpSpeed;
        //anim.SetTrigger("Jump");
        //anim.SetBool("isGrounded", false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(bottomOffset.position, collisionRadius);
    }

    public void HitPlayer(Transform source)
    {
        isRecoiling = true;
        Vector3 result;
        if (source.position.x > transform.position.x)
            result = Vector3.left;
        else
            result = Vector3.right;

        recoilDir = result;
        recoilTrack = Time.time + recoilTime;
    }

    public void Dash(float dashTime, float speed)
    {
        if (isRecoiling) return;
        isRecoiling = true;
        recoilDir = speed * (isFacingRight ? Vector3.right : Vector3.left);
        recoilTrack = Time.time + dashTime;
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
}