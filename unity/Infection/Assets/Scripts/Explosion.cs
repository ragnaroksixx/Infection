using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Explosion : MonoBehaviour
{
    public float stunTime = 0.1f;
    public float stunTimeZero = 0.1f;
    public AudioClip sfx;
    public float expolsiveforce;
    int damage;

    public void Init(float size, float duration, int d)
    {
        AudioManager.Play(sfx, transform.position);
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one * size, duration);
        Destroy(this.gameObject, duration);
        damage = d;
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        Shield s = other.GetComponent<Shield>();
        if (s)
        {
            s.TakeDamage();
        }
        else
        {
            Movement m = other.GetComponentInParent<Movement>();
            if (m is PlayerMovement && m.isRecoiling) return;
            ApplyAttackEffects(m);
        }

    }
    public virtual void ApplyAttackEffects(Movement target)
    {
        Vector3 result = target.transform.position - transform.position;
        result.Normalize();
        result *= expolsiveforce;
        target.HitCharacter(result, stunTime, stunTimeZero, damage);
        AudioManager.Play(sfx, transform.position);
    }
}

