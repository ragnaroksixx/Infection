using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



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
    public int numPhases;
    public void StartBattle()
    {
        ChangePhase(Phase.ONE);
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
    public void ChangePhase(Phase p)
    {
        ExitPhase(phase);
        phase = p;
        StartPhase(phase);
    }
    public void StartPhase(Phase p)
    {
        switch (p)
        {
            case Phase.NONE:
                break;
            case Phase.ONE:
                shooter.StartShooting();
                room.ShowFloor(false);
                break;
            case Phase.TWO:
                room.ShowFloor(true);
                foreach (Spawner item in spawners)
                {
                    item.reSpawnOnDie = true;
                    item.Spawn(room);
                }
                shieldInstance = GameObject.Instantiate(shieldPrefab, transform.position, Quaternion.identity);
                //shieldInstance.Init();
                break;
            case Phase.DEAD:
                break;
            default:
                break;
        }
        phaseStartTime = Time.time;
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
                break;
            case Phase.DEAD:
                break;
            default:
                break;
        }
    }


}

