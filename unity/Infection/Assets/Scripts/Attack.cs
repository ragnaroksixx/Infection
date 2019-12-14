using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float startUpLag,
        attackDuration,
        endLag,
        comboTime;
    public int damage;
    public Vector2 localDirection;
    public Collider hitbox;
    public MeshRenderer mesh;

    public Attack nextAttack;
    protected bool doNextCombo;
    public KeyCode key = KeyCode.Mouse0;
    public KeyCode controllerkey = KeyCode.Mouse0;
    public enum AttackState
    {
        None,
        StartUP,
        Attack,
        EndLag
    }
    protected AttackState state;
    protected Movement self;
    public float stunTime = 0.1f;
    public float stunTimeZero = 0.1f;
    public bool isGroundAttack = true;
    public bool isAirAttack = false;
    public bool IsAttacking
    {
        get
        {
            return state != AttackState.None || (nextAttack != null && nextAttack.IsAttacking);
        }
    }
    public string attackTrigger;
    public AudioClip sfx;
    // Start is called before the first frame update
    public virtual void Start()
    {
        attackDuration += startUpLag;
        endLag += attackDuration + startUpLag;
        comboTime += startUpLag + attackDuration + endLag;
        self = GetComponentInParent<Movement>();
        SetHitBox(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public virtual bool CanAttack()
    {
        return !IsAttacking && !self.IsSimulated;
    }
    public virtual void StartAttack()
    {
        doNextCombo = false;
        SetHitBox(true);
        StartCoroutine(AttackUpdate());
        if (!string.IsNullOrEmpty(attackTrigger))
        {
            self.anim.SetBool("attackDone", false);
            self.anim.SetBool("skip", false);
            self.anim.SetTrigger(attackTrigger);
        }
    }
    public virtual void EndAttack()
    {
        state = AttackState.None;
        if (!string.IsNullOrEmpty(attackTrigger))
        {
            SetHitBox(false);
            if (!doNextCombo)
                self.anim.SetBool("attackDone", true);
        }
        doNextCombo = false;
    }
    float time = 0;
    protected float timeAddition = 0;
    public IEnumerator AttackUpdate()
    {
        time = 0;
        timeAddition = 0;
        float startTime = Time.time;
        state = AttackState.StartUP;
        OnAttackStartUp();
        while (true)
        {
            time = Time.time - startTime + timeAddition;
            if (time < startUpLag)
            {
                OnStartUpUpdate();
            }
            else if (time <= attackDuration)
            {
                if (state == AttackState.StartUP)
                {
                    OnAttack();
                }
                OnAttackUpdate();
            }
            else if (time <= endLag)
            {
                if (state == AttackState.Attack)
                {
                    OnEndLagStart();
                }
                OnEndLagUpdate();
                if (doNextCombo)
                    break;
            }
            else
            {
                break;
            }

            if (nextAttack != null && nextAttack.KeyDown() && time > startUpLag)
            {
                doNextCombo = true;
            }
            yield return null;
        }
        if (doNextCombo)
            nextAttack.StartAttack();
        EndAttack();
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        Movement m = other.GetComponentInParent<Movement>();
        if (m is PlayerMovement && m.isRecoiling) return;
        if (m && m != self)
        {
            ApplyAttackEffects(m);
        }
    }
    public virtual void ApplyAttackEffects(Movement target)
    {
        Vector3 result = localDirection;
        if (!self.isFacingRight)
            result.x *= -1;
        target.HitCharacter(result, stunTime, stunTimeZero, damage);
        AudioManager.Play(sfx, self.transform.position);
    }
    public virtual void OnAttackStartUp()
    {
        if (self.isGrounded)
            self.SimulateInput(new Vector2(self.isFacingRight ? 1 : -1, 0));
    }

    public virtual void OnStartUpUpdate()
    {

    }
    public virtual void OnAttack()
    {
        self.StopSimulateInput();
        SetHitBox(true);
        state = AttackState.Attack;
    }
    public virtual void OnAttackUpdate()
    {

    }
    public virtual void OnEndLagStart()
    {
        SetHitBox(false);
        state = AttackState.EndLag;
    }
    public virtual void OnEndLagUpdate()
    {

    }

    public void SetHitBox(bool value)
    {
        hitbox.enabled = value;
        mesh.enabled = value;
    }

    public virtual void InterruptAttack()
    {
        if (state != AttackState.None)
        {
            SetHitBox(false);
            doNextCombo = false;
            timeAddition += 1000;
            self.StopSimulateInput();
            EndAttack();
        }
        if (nextAttack)
            nextAttack.InterruptAttack();
    }

    public bool KeyDown()
    {
        return Input.GetKeyDown(key) || Input.GetKeyDown(controllerkey);
    }
    public bool KeyUp()
    {
        return Input.GetKeyUp(key) || Input.GetKeyUp(controllerkey);
    }
}
