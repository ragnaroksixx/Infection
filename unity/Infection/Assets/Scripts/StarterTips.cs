using UnityEngine;
using System.Collections;
using DG.Tweening;

public class StarterTips : MonoBehaviour
{
    public Sprite sprite;
    public static bool isNewGame;
    IEnumerator Start()
    {
        if (!isNewGame)
        {
            Destroy(this.gameObject);
            yield break;
        }
        isNewGame = false;
        yield return new WaitForSeconds(2);
        CollectibleMenu.instance.SetVis(true, sprite);
    }
}

