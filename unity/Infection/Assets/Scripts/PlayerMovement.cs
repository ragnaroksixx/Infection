using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : Movement
{
    public static PlayerMovement instance;

    public Attack meleeAttack;
    public Attack ariealAttack;
    public ClawAttack clawAttack;
    public CorruptAttack corruptAttack;
    public Transform graphic;
    public Mesh wArm, woArm;
    public SkinnedMeshRenderer skinnedMesh;
    public GameObject holdArm, grabArm;
    public override void SetController(InputController ic)
    {
        base.SetController(ic);
        if (ic)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }
    public override void DetermineInput()
    {
        base.DetermineInput();
    }
    public override void Awake()
    {
        maxHP = SaveLoad.GetMaxHP();
        numAirJumps = SaveLoad.NumAirJumps();
        base.Awake();
        instance = this;
        UseAttachedArm();
        SetAttacks(meleeAttack, ariealAttack, clawAttack, corruptAttack);
    }
    public override void Update()
    {
        base.Update();

    }
    public override void FixedUpdate()
    {
        if (PlayerInputController.instance.IsCorrupting)
        {
            transform.position = PlayerInputController.instance.CorruptingEnemy.transform.position;
        }
        else
            base.FixedUpdate();
    }
    public override bool IsAttacking()
    {
        return meleeAttack.IsAttacking || ariealAttack.IsAttacking || clawAttack.IsAttacking || corruptAttack.IsAttacking;
    }

    public void Hide()
    {
        graphic.gameObject.SetActive(false);
        FreezeRBody();
    }

    public void Show()
    {
        graphic.gameObject.SetActive(true);
        UnFreezeRBody();
    }
    public override void StopRecoil()
    {
        base.StopRecoil();
        isInvincible = false;
    }
    public override void Die(bool ignoreSpawn)
    {
        SaveLoad.ReloadScene();
        base.Die(ignoreSpawn);
    }
    public void UseSeperateArm(bool hArm)
    {
        skinnedMesh.sharedMesh = woArm;
        holdArm.SetActive(hArm);
        grabArm.SetActive(!hArm);
    }

    public void UseAttachedArm()
    {
        skinnedMesh.sharedMesh = wArm;
        holdArm.SetActive(false);
        grabArm.SetActive(false);
    }

    public override void HitCharacter(Vector3 dir, float stunTime, float zeroVelocityTime, int damage)
    {
        base.HitCharacter(dir, stunTime, zeroVelocityTime, damage);
        if (damage != 0)
            isInvincible = true;
    }
}
