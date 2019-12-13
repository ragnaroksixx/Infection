using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    [HideInInspector]
    public Vector2 input;
    Vector2 simulatedInput;
    protected Rigidbody rBody;

    public float fallMult = 2.5f;

    public float lowJumpMult = 2.0f;

    public bool isGrounded = false;
    public bool wasGrounded;
    public LayerMask groundLayer;
    protected float collisionRadius = .2f;
    public Transform bottomOffset;
    public bool isFacingRight = true;
    public float jumpHoldTime = 1;
    protected float jumpHoldTimeTrack;
    bool holdingJump = true;

    public float coyoteTime = 0.1f;
    public float coyoteTimeTrack;
    float recoilTrack;
    float zeroRecoilTrack;
    public bool isRecoiling;
    private bool isSimulated;
    Vector2 recoilDir;
    public float drag = 1;
    public Animator anim;
    bool fastFall;
    public float fastFallMult = 8f;
    public bool IsSimulated { get => isSimulated; set => isSimulated = value; }
    public float CollisionRadius { get => collisionRadius; set => collisionRadius = value; }
    public Health Health { get => health; set => health = value; }
    public Rigidbody RBody { get => rBody; }
    public Spawner Spawn { get => spawn; set => spawn = value; }

    public InputController controller;
    public int maxJumps = 1;
    int jumpTrack = 0;
    [HideInInspector]
    public Attack[] attacks;
    public int maxHP;
    Health health;
    public int targetPriority;
    public AudioClip jumpSFX, hitSFX, dieSFX;
    Spawner spawn;
    DamagedFlasher flasher;
    public HealthUI ui;
    public void SetAttacks(params Attack[] atks)
    {
        attacks = atks;
    }
    public virtual void Awake()
    {
        rBody = GetComponent<Rigidbody>();
        flasher = GetComponentInChildren<DamagedFlasher>();
        defaultConstraints = RBody.constraints;
        health = new Health(maxHP);
        if (ui)
            ui.UpdateUI(health);
    }
    public virtual void Start()
    {
        jumpTrack = maxJumps;
        //anim = GetComponent<Animator>();
    }
    public virtual void Update()
    {
        OnUpdate();
    }
    public virtual void DetermineInput()
    {
        input = Vector2.zero;
        if (isRecoiling)
        {
            if (Time.time > recoilTrack)
                input = Vector2.zero;
            else
                input = recoilDir;
        }
        else
        {
            if (IsSimulated)
            {
                input = simulatedInput;
            }
            else
            {
                if (controller)
                    controller.SetInput(this);
            }
        }
    }
    public virtual void OnUpdate()
    {
        wasGrounded = isGrounded;
        isGrounded = Physics.OverlapSphere(bottomOffset.position, collisionRadius, groundLayer).Length > 0;
        if (!wasGrounded && isGrounded)
        {
            OnGrounded();
        }
        if (anim)
            anim.SetBool("isGrounded", isGrounded);

        DetermineInput();

        if (isRecoiling)
        {
            if (Time.time > zeroRecoilTrack)
            {
                isRecoiling = false;
            }
            SetVelocity(input, 1, false);
        }
        else
        {
            if (input.x > 0 && !isFacingRight)
                FaceDirection(true);
            else if (input.x < 0 && isFacingRight)
                FaceDirection(false);
            SetVelocity(input, speed, true);
            if (anim)
                anim.SetBool("isRunning", Mathf.Abs(input.x) > 0);
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
        else if (controller)
            controller.SetAttacks(this);

    }
    public virtual void SetController(InputController ic)
    {
        controller = ic;
    }
    public virtual bool IsHoldingJump()
    {
        return jumpHoldTimeTrack > 0 && controller.IsHoldingJump(this);
    }

    public virtual bool ShouldJump()
    {
        bool canJump = jumpTrack > 0 || Time.time < coyoteTimeTrack;
        canJump = canJump && !isSimulated;
        canJump = canJump && (controller && controller.Jump(this));
        canJump = canJump && !IsAttacking() && !isRecoiling;
        return canJump;
    }
    public virtual void OnFallingDown()
    {
        if (controller && controller.IsFastFall(this))
            fastFall = true;
        if (fastFall)
            rBody.velocity += Vector3.up * Physics.gravity.y * (fastFallMult - 1) * Time.deltaTime;
        else
            rBody.velocity += Vector3.up * Physics.gravity.y * (fallMult - 1) * Time.deltaTime;
    }
    public virtual void OnGrounded()
    {
        coyoteTimeTrack = Time.time + coyoteTime;
        fastFall = false;
        jumpTrack = maxJumps;
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

    public void FaceDirection(bool right)
    {
        isFacingRight = right;
        transform.localEulerAngles = new Vector3(0, right ? 0 : 180, 0);
    }

    public virtual void Jump()
    {
        jumpHoldTimeTrack = jumpHoldTime;
        rBody.velocity = new Vector2(rBody.velocity.x, 0);
        rBody.velocity += Vector3.up * jumpSpeed;
        jumpTrack--;
        if (anim)
            anim.SetTrigger("jump");
        AudioManager.Play(jumpSFX, transform.position);
    }

    public virtual void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(bottomOffset.position, collisionRadius);
    }

    public virtual void HitCharacter(Vector3 dir, float stunTime, float zeroVelocityTime, int damage)
    {
        isRecoiling = true;
        recoilDir = dir;
        recoilTrack = Time.time + stunTime;
        zeroRecoilTrack = recoilTrack + zeroVelocityTime;
        health.LoseHP(damage);
        if (ui)
            ui.UpdateUI(health);
        if (health.currentHP <= 0)
        {
            Die();
        }
        else
        {
            AudioManager.Play(hitSFX, transform.position);
            if (flasher && damage > 0)
            {
                flasher.Flash(stunTime + zeroVelocityTime);
            }
        }
        InterruptAttack();
    }

    public void InterruptAttack()
    {
        if (IsAttacking())
        {
            foreach (Attack attack in attacks)
            {
                attack.InterruptAttack();
            }
        }
    }
    public void SimulateInput(Vector2 i)
    {
        IsSimulated = true;
        simulatedInput = i;
    }

    public void StopSimulateInput()
    {
        IsSimulated = false;
    }

    public virtual bool IsAttacking()
    {
        return false;
    }

    public virtual void Die(bool ignoreSpawn = false)
    {
        if (spawn && !ignoreSpawn)
        {
            spawn.OnDieCallback();
        }
        Destroy(this.gameObject);
        AudioManager.Play(dieSFX, transform.position);
    }
    RigidbodyConstraints defaultConstraints;
    public void FreezeRBody()
    {
        rBody.useGravity = false;
        rBody.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void UnFreezeRBody()
    {
        rBody.useGravity = true;
        rBody.constraints = defaultConstraints;
    }
}

