using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorruptableObject : Movement, ICorruptable, IGrabable
{
    public Explosion explosionPrefab;
    public GameObject effects;
    public ObstructionHandler oHandler;
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
        return Health.currentHP <= 2;
    }
    public bool CanCorrupt()
    {
        return CanGrab();
    }
    public bool CanRelease()
    {
        return !oHandler.IsBlocked();
    }
    public override void HitCharacter(Vector3 dir, float stunTime, float zeroVelocityTime, int damage)
    {
        base.HitCharacter(dir, stunTime, zeroVelocityTime, damage);
        if (CanGrab() && !effects.activeInHierarchy)
            effects.SetActive(true);
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

public class CoRunner : MonoBehaviour
{

}

