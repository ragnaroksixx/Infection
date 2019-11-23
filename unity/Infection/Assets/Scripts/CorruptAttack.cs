using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CorruptAttack : Attack
{

    public InputController playerController;
    public override void ApplyAttackEffects(Movement target)
    {
        if (PlayerInputController.instance.IsCorrupting) return;
        //base.ApplyAttackEffects(target);
        PlayerInputController.instance.OnCorrupt(target);
        self.SetController(null);
        target.SetController(playerController);
        target.isRecoiling = false;
        //Room.SetCameraTarget(target.transform);
    }
    public override bool CanAttack()
    {
        return base.CanAttack() && !PlayerInputController.instance.IsHoldingObject;
    }
}

