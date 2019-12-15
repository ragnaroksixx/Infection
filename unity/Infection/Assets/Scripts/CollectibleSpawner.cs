using UnityEngine;
using System.Collections;

public class CollectibleSpawner : Spawner
{
    public string t;
    public Sprite sprite;
    public Sprite collectibleUI;
    Collectible cInstance;
    public override GameObject Spawn(Room r)
    {
        if (SaveLoad.HasCollectible(t)) return null;
        GameObject g = base.Spawn(r);
        cInstance = g.GetComponent<Collectible>();
        cInstance.idTag = t;
        cInstance.render.sprite = sprite;
        cInstance.Ui = collectibleUI;
        return g;
    }
    public override void Kill()
    {
        base.Kill();
        if(cInstance)
        {
            Destroy(cInstance.gameObject);
            cInstance = null;
        }
    }
}
