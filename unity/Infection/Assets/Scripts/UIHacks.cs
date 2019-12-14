using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHacks : MonoBehaviour
{
    public HealthUI player, corrupt, boss;
    public GameObject miniMap;
    public static UIHacks Instance;
    private void Awake()
    {
        Instance = this;
        SetCorruptable(false);
        SetBoss(false);
    }
    public void SetPlayer(bool value)
    {
        player.gameObject.SetActive(value);
    }

    public void SetCorruptable(bool value)
    {
        corrupt.gameObject.SetActive(value);
    }

    public void SetBoss(bool value)
    {
        boss.gameObject.SetActive(value);
    }
    public void SetMinimap(bool value)
    {
        miniMap.gameObject.SetActive(value);
    }
}

