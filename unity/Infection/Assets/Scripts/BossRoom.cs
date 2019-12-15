using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class BossRoom : Room
{
    public Transform safeFloor;
    public Vector3 upPos, downPos;
    public float floorSpeed;
    public override void Start()
    {
        base.Start();
        ShowFloor(false);
    }
    [Button]
    public void ShowFloor(bool val)
    {
        Vector3 pos = val ? upPos : downPos;
        safeFloor.DOLocalMove(pos, floorSpeed);
    }
    public override void Enter()
    {
        foreach (Spawner spawner in spawners)
        {
            spawner.Spawn(this);
        }
        if (points)
        {
            Cleaner.instance.Start(points);
        }
        else
        {
            Cleaner.instance.Stop();
        }
        vCam.Priority = focusPriority;
        AudioManager.SetBgm(overrideBGM);
        playerRoom = this;
        UIHacks.Instance.SetBoss(true);
        UIHacks.Instance.SetMinimap(false);
        GetComponentInChildren<BossPhaseController>().StartBattle();
    }
}
