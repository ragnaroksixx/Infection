using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public List<Image> images;
    public Sprite full, empty;
    public void UpdateUI(Health hp)
    {
        int i = 0;
        foreach (Image image in images)
        {
            i++;
            if (i <= hp.currentHP)
                image.sprite = full;
            else if (i <= hp.maxHP)
                image.sprite = empty;

            image.enabled = i <= hp.maxHP;
        }
    }
}
