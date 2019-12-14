using UnityEngine;
using System.Collections;

public class HPCollectible : Collectible
{

    public override void OnCollect()
    {
        base.OnCollect();
        SaveLoad.IncreaseMaxHP();
        PlayerMovement.instance.Health.IncreaseMaxHP(1);
        PlayerMovement.instance.ui.UpdateUI(PlayerMovement.instance.Health);
    }
}
