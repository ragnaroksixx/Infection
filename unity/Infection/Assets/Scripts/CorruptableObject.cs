using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorruptableObject : Movement, ICorruptable, IGrabable
{
    public Explosion explosionPrefab;

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
        destroyOnTouch = true;
    }
    public bool CanGrab()
    {
        return Health.currentHP <= 1;
    }
    bool destroyOnTouch;
    public void OnCollisionEnter(Collision collision)
    {
        if (destroyOnTouch)
        {
            GameObject.Instantiate(explosionPrefab, collision.contacts[0].point, Quaternion.identity).Init(4, 1, 1);
            Die(false);
        }
    }
}

