using UnityEngine;
using System.Collections;

public class EnemyHitbox : MonoBehaviour
{
    protected Movement self;
    public float recoilSpeed = 5;
    public float recoilTime = 0.5f;
    public float zeroRecoilTime = 0.2f;
    // Use this for initialization
    void Start()
    {
        self = GetComponentInParent<Movement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement m = other.GetComponentInParent<PlayerMovement>();
        if (m == null) return;
        if (self.isRecoiling) return;
        Vector2 dir;
        dir = m.bottomOffset.transform.position - self.bottomOffset.transform.position;
        dir.Normalize();
        int damage = 1;
        if (m.isRecoiling)
            damage = 0;
        if (m && m != self)
        {
            m.HitCharacter(dir * recoilSpeed, recoilTime, zeroRecoilTime);
        }

    }
}
