using UnityEngine;
using System.Collections;

public class HPCollectibleMEGA : Collectible
{

    public override void OnCollect()
    {
        base.OnCollect();
        SaveLoad.IncreaseMaxHP();
        PlayerMovement.instance.Health.IncreaseMaxHP(5);
        PlayerMovement.instance.ui.UpdateUI(PlayerMovement.instance.Health);
    }
}
