using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerInputController : InputController
{
    public PlayerMovement originalPlayer;
    public static PlayerInputController instance;
    Movement corruptingEnemy = null;
    Movement holdingEnemy = null;
    public AudioClip cantReleaseSFX;
    public void Awake()
    {
        instance = this;
    }
    public override void SetInput(Movement m)
    {
        if (!m.IsSimulated && !m.isRecoiling && !canRelease)
        {
            if (m.IsAttacking() && m.isGrounded)
            {
                return;
            }
            if (InputHandler.IsMovingLeft())
            {
                m.input.x -= 1;
            }
            if (InputHandler.IsMovingRight())
            {
                m.input.x += 1;
            }
        }
    }
    public override void SetAttacks(Movement m)
    {
        base.SetAttacks(m);
        if (!m.IsAttacking() && !canRelease)
        {
            foreach (Attack attack in m.attacks)
            {
                if (!attack.CanAttack()) continue;
                if ((attack.isGroundAttack && m.isGrounded || attack.isAirAttack && !m.isGrounded) &&
                    attack.KeyDown())
                {
                    attack.StartAttack();
                    break;
                }
            }
        }
    }
    public override bool IsHoldingJump(Movement m)
    {
        return InputHandler.IsJumpKey();
    }

    public override bool IsFastFall(Movement m)
    {
        return InputHandler.IsFastFall();
    }

    public override bool Jump(Movement m)
    {
        return InputHandler.IsJumpKeyDown() && !canRelease;
    }

    bool canRelease;
    public float absorbTime;
    float absorbTimeTrack;

    public bool IsCorrupting { get => corruptingEnemy != null; }
    public bool IsHoldingObject { get => holdingEnemy != null; }
    public CorruptableObject CorruptingEnemy { get => corruptingEnemy as CorruptableObject; set => corruptingEnemy = value; }
    public Movement HoldingObject { get => holdingEnemy; set => holdingEnemy = value; }

    public override void Update()
    {
        base.Update();
        if (IsCorrupting)
        {
            if (!canRelease && originalPlayer.corruptAttack.KeyDown())
            {
                if (!CorruptingEnemy.CanRelease())
                {
                    if (CorruptingEnemy.Flasher)
                    {
                        CorruptingEnemy.Flasher.Flash(0.5f);
                        AudioManager.Play(cantReleaseSFX, transform.position);
                    }
                }
                else
                {
                    canRelease = true;
                    absorbTimeTrack = 0;
                }
            }
            if (canRelease)
            {
                if (originalPlayer.corruptAttack.KeyUp() || absorbTimeTrack >= absorbTime)
                {
                    canRelease = false;
                    if (absorbTimeTrack < absorbTime)
                    {
                        corruptingEnemy.SetController(null);
                        corruptingEnemy.HitCharacter(Vector3.up * 5, .2f, 3, 0);
                    }
                    else
                    {
                        corruptingEnemy.Die();
                        originalPlayer.Health.GainHP(1);
                        UIHacks.Instance.player.UpdateUI(originalPlayer.Health);
                    }
                    ReleasCorruption();
                }
                else
                {
                    absorbTimeTrack += Time.deltaTime;
                }
            }
        }
    }
    public void ReleasCorruption()
    {
        if (corruptingEnemy)
        {
            corruptingEnemy.ui = null;
            corruptingEnemy.SetController(corruptingEnemy.GetComponent<InputController>());
        }
        corruptingEnemy = null;
        originalPlayer.SetController(this);
        UIHacks.Instance.SetCorruptable(false);
    }
    public void OnCorrupt(Movement m)
    {
        canRelease = false;
        corruptingEnemy = m;
        corruptingEnemy.ui = UIHacks.Instance.corrupt;
        UIHacks.Instance.corrupt.UpdateUI(corruptingEnemy.Health);
        UIHacks.Instance.SetCorruptable(true);
        Door.CloseAllDoors();
    }
}
