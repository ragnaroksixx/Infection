﻿using System.Collections;
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
    bool doNextCombo;
    public KeyCode key = KeyCode.Mouse0;
    public enum AttackState
    {
        None,
        StartUP,
        Attack,
        EndLag
    }
    AttackState state;
    protected Movement self;
    public float stunTime = 0.1f;
    public bool IsAttacking
    {
        get
        {
            return state != AttackState.None || (nextAttack != null && nextAttack.IsAttacking);
        }
    }
    // Start is called before the first frame update
    public virtual void Start()
    {
        attackDuration += startUpLag;
        endLag += attackDuration + startUpLag;
        comboTime += startUpLag + attackDuration + endLag;
        self = GetComponentInParent<Movement>();
        hitbox.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void StartAttack()
    {
        doNextCombo = false;
        hitbox.gameObject.SetActive(true);
        StartCoroutine(AttackUpdate());
    }
    public virtual void EndAttack()
    {
        doNextCombo = false;
        state = AttackState.None;
        hitbox.gameObject.SetActive(false);
    }
    public IEnumerator AttackUpdate()
    {
        float time = 0;
        float startTime = Time.time;
        state = AttackState.StartUP;
        OnAttackStartUp();
        while (true)
        {
            time = Time.time - startTime;
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

            if (nextAttack != null && Input.GetKeyDown(nextAttack.key) && time > startUpLag)
            {
                doNextCombo = true;
            }
            yield return null;
        }
        if (doNextCombo)
            nextAttack.StartAttack();
        EndAttack();
    }
    private void OnTriggerEnter(Collider other)
    {
        Movement m = other.GetComponentInParent<Movement>();
        if (m && m != self && !m.isRecoiling)
        {
            Vector3 result = localDirection;
            if (!self.isFacingRight)
                result.x *= -1;
            m.HitCharacter(result, stunTime);
        }

    }
    public virtual void OnAttackStartUp()
    {
        self.SimulateInput(new Vector2(self.isFacingRight ? 1 : -1, 0));
    }

    public virtual void OnStartUpUpdate()
    {

    }
    public virtual void OnAttack()
    {
        self.StopSimulateInput();
        hitbox.enabled = mesh.enabled = true;
        state = AttackState.Attack;
    }
    public virtual void OnAttackUpdate()
    {

    }
    public virtual void OnEndLagStart()
    {
        hitbox.enabled = mesh.enabled = false;
        state = AttackState.EndLag;
    }
    public virtual void OnEndLagUpdate()
    {

    }
}