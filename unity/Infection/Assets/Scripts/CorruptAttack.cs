using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CorruptAttack : Attack
{
    public bool isCorrupting = false;
    public InputController playerController;
    public override void ApplyAttackEffects(Movement target)
    {
        if (isCorrupting) return;
        //base.ApplyAttackEffects(target);
        isCorrupting = true;
        self.SetController(null);
        playerController.SetMovement(target);
        //Room.SetCameraTarget(target.transform);
    }
}

