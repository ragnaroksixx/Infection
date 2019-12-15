using UnityEngine;
using System.Collections;

public class EnemyHitbox : MonoBehaviour
{
    protected Movement self;
    public float recoilSpeed = 5;
    public float recoilTime = 0.5f;
    public float zeroRecoilTime = 0.2f;
    public int damage = 1;
    Patrol patrol;
    // Use this for initialization
    void Start()
    {
        self = GetComponentInParent<Movement>();
        patrol = GetComponentInParent<Patrol>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement m = other.GetComponentInParent<PlayerMovement>();
        if (m == null) return;
        if (self.isRecoiling) return;
        if (self.controller == null) return;
        Vector2 dir;
        dir = m.bottomOffset.transform.position - self.bottomOffset.transform.position;
        dir.Normalize();
        int damageTemp = damage;
        if (m.isRecoiling)
            damageTemp = 0;
        if (m && m != self)
        {
            m.HitCharacter(dir * recoilSpeed, recoilTime, zeroRecoilTime, damageTemp);
        }
        Patrol patrol = self.GetComponent<Patrol>();
        if (patrol)
        {
            bool isPlayerInFront;
            if (self.isFacingRight)
                isPlayerInFront = m.transform.position.x > self.transform.position.x;
            else
                isPlayerInFront = m.transform.position.x < self.transform.position.x;

            if (isPlayerInFront)
            {
                self.FaceDirection(!self.isFacingRight);
            }
        }

    }
}
