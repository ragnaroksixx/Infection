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
        rBody.useGravity = false;
        rBody.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void Show()
    {
        graphic.gameObject.SetActive(true);
        rBody.useGravity = true;
        rBody.constraints = RigidbodyConstraints.FreezeRotation;
    }
}
