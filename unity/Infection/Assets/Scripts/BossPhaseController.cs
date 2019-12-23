using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

public class BossPhaseController : MonoBehaviour
{
    public enum Phase
    {
        NONE,
        ONE,
        TWO,
        DEAD
    }
    public BossShooter shooter;
    public BossRoom room;
    public Spawner[] spawners;
    public Phase phase;
    public Movement boss;
    float phaseStartTime;
    int healthCheck;
    public Shield shieldPrefab;
    Shield shieldInstance;
    public int numPhases = 4;
    public GameObject[] grapplePoints;
    public Transform shieldGraphic;
    public float startOverDelay = 3;
    private void Start()
    {
        foreach (Spawner item in spawners)
        {
            item.reSpawnOnDie = false;
            item.Kill();
        }
    }
    public void StartBattle()
    {
        foreach (Spawner item in spawners)
        {
            item.reSpawnOnDie = false;
            item.Kill();
        }
        ChangePhase(Phase.ONE);
    }
    private void Update()
    {
        PhaseUpdate();
    }
    public void PhaseUpdate()
    {
        if (boss.Health.currentHP <= 0)
        {
            ChangePhase(Phase.DEAD);
            return;
        }
        switch (phase)
        {
            case Phase.NONE:
                break;
            case Phase.ONE:
                if (boss.Health.currentHP <= healthCheck)
                    ChangePhase(Phase.TWO);
                break;
            case Phase.TWO:
                if (shieldInstance == null)
                    ChangePhase(Phase.ONE);
                break;
            case Phase.DEAD:
                break;
            default:
                break;
        }
    }
    [Button]
    public void ChangePhase(Phase p)
    {
        ExitPhase(phase);
        phase = p;
        StartPhase(phase);
    }
    public void StartPhase(Phase p)
    {
        boss.isInvincible = false;
        switch (p)
        {
            case Phase.NONE:
                break;
            case Phase.ONE:
                shooter.StartShooting();
                room.ShowFloor(false);
                foreach (GameObject g in grapplePoints)
                {
                    g.SetActive(true);
                }
                healthCheck = boss.Health.currentHP - (boss.Health.maxHP / numPhases);
                break;
            case Phase.TWO:
                room.ShowFloor(true);
                foreach (Spawner item in spawners)
                {
                    item.reSpawnOnDie = true;
                    item.Spawn(room);
                }
                foreach (GameObject g in grapplePoints)
                {
                    g.SetActive(false);
                }
                shieldInstance = GameObject.Instantiate(shieldPrefab, transform.position, Quaternion.identity);
                shieldInstance.Init(new Vector3(15, 6, 3));
                boss.isInvincible = true;
                shieldGraphic.DOLocalMove(new Vector3(0, 0, -.5f), 0.5f);
                break;
            case Phase.DEAD:
                SaveLoad.MainMenu(startOverDelay);
                break;
            default:
                break;
        }
        phaseStartTime = Time.time;
    }
    private void OnDestroy()
    {
        SaveLoad.MainMenu(startOverDelay);
    }
    public void ExitPhase(Phase p)
    {
        switch (p)
        {
            case Phase.NONE:
                break;
            case Phase.ONE:
                shooter.Stop();
                break;
            case Phase.TWO:
                foreach (Spawner item in spawners)
                {
                    item.reSpawnOnDie = false;
                    item.Kill();
                }
                shieldGraphic.DOLocalMove(new Vector3(0, 5, -.5f), 0.5f);
                break;
            case Phase.DEAD:
                break;
            default:
                break;
        }
    }


}

