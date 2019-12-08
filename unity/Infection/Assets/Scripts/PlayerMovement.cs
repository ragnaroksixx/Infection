﻿using System;
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
        base.Awake();
        instance = this;
        UseAttachedArm();
        SetAttacks(meleeAttack, ariealAttack, clawAttack, corruptAttack);
    }
    public override void Update()
    {
        base.Update();

    }
    private void FixedUpdate()
    {
        if (PlayerInputController.instance.IsCorrupting)
        {
            transform.position = PlayerInputController.instance.CorruptingEnemy.transform.position;
        }
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

    public override void Die()
    {
        SaveLoad.ReloadScene();
        base.Die();
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
}
