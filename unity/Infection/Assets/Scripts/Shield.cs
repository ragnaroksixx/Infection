using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Shield : MonoBehaviour
{
    public int HP = 3;
    DamagedFlasher flasher;
    private void Awake()
    {
        flasher = GetComponent<DamagedFlasher>();
    }
    public void Init(Vector3 size)
    {
        transform.localScale = new Vector3(0, size.y, 0);
        transform.DOScale(size, 1.25f);
    }

    public void TakeDamage()
    {
        HP--;
        flasher.Flash(.75f);
        if (HP <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        Destroy(this.gameObject);
    }
}
