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

    public override void SetController(InputController ic)
    {
        base.SetController(ic);
        ic.AddAttacks(meleeAttack, ariealAttack, clawAttack);
    }
    private void Awake()
    {
        instance = this;
    }

    public override bool IsAttacking()
    {
        return meleeAttack.IsAttacking || ariealAttack.IsAttacking || clawAttack.IsAttacking;
    }


}
