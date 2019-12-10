using UnityEngine;
using System.Collections;

public class WormHitBox : MonoBehaviour
{
    public int damage = 1;

    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement m = other.GetComponentInParent<PlayerMovement>();
        if (m == null) return;
        m.HitCharacter(Vector3.zero, 0, 0, damage);
    }
}
