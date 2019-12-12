using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorruptableObject : Movement, ICorruptable, IGrabable
{
    public override void Die(bool ignoreSpawn)
    {
        if (PlayerInputController.instance.CorruptingEnemy == this)
        {
            PlayerInputController.instance.ReleasCorruption();
        }
        base.Die(ignoreSpawn);

    }
    public Collider[] cols;
    public void OnGrab()
    {
        foreach (Collider col in cols)
            col.enabled = false;
    }

    public void OnThrow()
    {
        foreach (Collider col in cols)
            col.enabled = true;
    }
    public bool CanGrab()
    {
        return Health.currentHP <= 1;
    }
}

