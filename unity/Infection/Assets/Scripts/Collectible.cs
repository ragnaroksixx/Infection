using UnityEngine;
using System.Collections;

public class Collectible : MonoBehaviour
{
    public string idTag;

    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement m = other.GetComponentInParent<PlayerMovement>();
        OnCollect();
        Destroy(this.gameObject);

    }
    public virtual void OnCollect()
    {
        SaveLoad.Collect(this);
    }
}
