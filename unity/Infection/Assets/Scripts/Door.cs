﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using Cinemachine;

public class Door : MonoBehaviour
{
    static List<Door> allDoors = new List<Door>();
    Vector3 closePos = new Vector3(0, 0, 0);
    public Vector3 openPos = new Vector3(0, 0, -3);
    float speed = 0.25f;
    public bool isLocked { get => lockId != -1; }
    public CinemachineVirtualCamera VCam { get => vCam; set => vCam = value; }

    public int lockId = -1;
    public KeyType keyType;
    CinemachineVirtualCamera vCam;
    public AudioClip open, locked;
    public MeshRenderer lockedDoorMesh;
    private void Awake()
    {
        allDoors.Add(this);
        vCam = GetComponentInChildren<CinemachineVirtualCamera>();
        if (isLocked && keyType == KeyType.NONE)
            Debug.LogError("Set key type!!!", this.gameObject);
        if (isLocked && SaveLoad.IsUnLocked(this))
        {
            lockId = -1;
        }
        else if (isLocked)
        {
            lockedDoorMesh.material.SetColor("_EmissionColor", Color.red * 44.2f);
        }
    }
    public static void CloseAllDoors()
    {
        foreach (Door d in allDoors)
        {
            if (d)
                d.Close();
        }
    }
    public void Open(bool ignoreLock = false)
    {
        if (isLocked && !ignoreLock)
        {
            AudioManager.Play(locked, transform.position);
            return;
        }
        if (!ignoreLock)
            AudioManager.Play(open, transform.position);
        transform.DOLocalMove(openPos, speed).SetDelay(0.25f);
    }

    public void Close()
    {
        transform.DOLocalMove(closePos, speed);
    }
    [Button]
    public void Unlock()
    {
        SaveLoad.UnlockDoor(this);
        lockId = -1;
        lockedDoorMesh.material.SetColor("_EmissionColor", Color.white);
    }
    private void OnDestroy()
    {
        allDoors = new List<Door>();
    }
}

