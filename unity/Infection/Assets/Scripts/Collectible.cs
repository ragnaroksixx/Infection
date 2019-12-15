using UnityEngine;
using System.Collections;

public class Collectible : MonoBehaviour
{
    public string idTag;
    public SpriteRenderer render;
    Sprite ui;

    public Sprite Ui { get => ui; set => ui = value; }

    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement m = other.GetComponentInParent<PlayerMovement>();
        OnCollect();
        Destroy(this.gameObject);
        if (ui)
            CollectibleMenu.instance.SetVis(true, ui);

    }
    public virtual void OnCollect()
    {
        SaveLoad.Collect(this);
    }
}
