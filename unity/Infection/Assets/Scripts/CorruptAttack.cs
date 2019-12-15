using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CorruptAttack : Attack
{

    public InputController playerController;
    bool didHitTarget = false;
    protected override void OnTriggerEnter(Collider other)
    {
        Door d = other.attachedRigidbody.GetComponent<Door>();
        if (d)
        {
            self.StopSimulateInput();
            SetHitBox(false);
            self.anim.SetBool("skip", true);
            timeAddition += 1000;
            d.Open();
        }
        else
            base.OnTriggerEnter(other);
    }
    public override void ApplyAttackEffects(Movement target)
    {
        if (PlayerInputController.instance.IsCorrupting) return;
        if (!(target is ICorruptable)) return;
        if (state == AttackState.StartUP && !didHitTarget)
        {
            target.HitCharacter(Vector3.one * 0.001f, comboTime, 0, 0);
            SetHitBox(false);
            didHitTarget = true;
        }
        else
        {
            //base.ApplyAttackEffects(target);
            AudioManager.Play(sfx, self.transform.position);
            PlayerInputController.instance.OnCorrupt(target);
            self.SetController(null);
            target.SetController(playerController);
            target.StopRecoil();
            EndAttack();
        }
        //Room.SetCameraTarget(target.transform);
    }

    public override void OnAttack()
    {
        self.StopSimulateInput();
        SetHitBox(false);
        state = AttackState.Attack;
        if (!didHitTarget)
        {
            timeAddition = attackDuration - startUpLag;
        }
    }
    public override void StartAttack()
    {
        doNextCombo = false;
        SetHitBox(false);
        //hitbox.gameObject.SetActive(true);
        StartCoroutine(AttackUpdate());
        if (!string.IsNullOrEmpty(attackTrigger))
        {
            self.anim.SetBool("attackDone", false);
            self.anim.SetTrigger(attackTrigger);
        }
    }
    public override void OnAttackStartUp()
    {
        //base.OnAttackStartUp();
        self.SimulateInput(new Vector2(0, 0));
        didHitTarget = false;
        SetHitBox(true);
    }
    public override void OnEndLagStart()
    {
        base.OnEndLagStart();
        SetHitBox(didHitTarget);
        self.anim.SetBool("attackDone", true);
    }

    public override bool CanAttack()
    {
        return base.CanAttack() && !PlayerInputController.instance.IsHoldingObject;
    }
}

