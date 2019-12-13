using UnityEngine;
using System.Collections;

public class CollectibleSpawner : Spawner
{
    public string t;
    public override GameObject Spawn(Room r)
    {
        if (SaveLoad.HasCollectible(t)) return null;
        GameObject g = base.Spawn(r);
        Collectible c = g.GetComponent<Collectible>();
        c.idTag = t;
        return g;
    }
}
