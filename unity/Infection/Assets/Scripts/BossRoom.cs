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
        base.Enter();
        UIHacks.Instance.SetBoss(true);
    }
}
