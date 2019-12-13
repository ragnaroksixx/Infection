using UnityEngine;
using System.Collections;

public class HPCollectible : Collectible
{

    public override void OnCollect()
    {
        base.OnCollect();
        SaveLoad.IncreaseMaxHP();
        PlayerMovement.instance.Health.IncreaseMaxHP(1);
    }
}
