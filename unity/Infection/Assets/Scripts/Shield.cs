using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Shield : MonoBehaviour
{
    public int HP = 3;
    public void Init(Vector3 size)
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(size, 1.25f);
    }

    public void TakeDamage()
    {
        HP--;
        if (HP <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        Destroy(this.gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {

    }
}
