using UnityEngine;
using System.Collections;
using Cinemachine;

public class Room : MonoBehaviour
{
    public const int focusPriority = 1;
    public const int unFocusPriority = 0;
    protected CinemachineVirtualCamera vCam;
    public static Room playerRoom;
    public int id = -1;
    Transform spawnPoint;
    protected Spawner[] spawners;
    protected CleanPoints points;
    public AudioClip overrideBGM;

    public Transform SpawnPoint { get => spawnPoint; set => spawnPoint = value; }

    public virtual void Start()
    {
        if (id == -1)
        {
            Debug.LogError("No Id set", this.gameObject);
        }
        spawnPoint = transform.Find("RoomStuff").Find("Spawn");
        if (spawnPoint == null)
        {
            Debug.LogError("No Spawn found", this.gameObject);
        }
        vCam = transform.Find("RoomStuff").GetComponentInChildren<CinemachineVirtualCamera>();
        if (vCam.Follow == null)
        {
            vCam.Follow = CameraTarget.Instance.transform;
        }
        spawners = GetComponentsInChildren<Spawner>();
        points = GetComponentInChildren<CleanPoints>();
        if (SaveLoad.spawnRoom == id)
        {
            Enter();
            PlayerMovement.instance.transform.position = spawnPoint.transform.position;
            Cleaner.instance.Stop();
        }
        else
        {
            vCam.Priority = unFocusPriority;
        }

    }
    public virtual void Enter()
    {
        if (playerRoom)
        {
            vCam.Follow = playerRoom.vCam.Follow;
        }
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

    }

    public void Exit()
    {
        vCam.Priority = unFocusPriority;
        foreach (Spawner spawner in spawners)
        {
            spawner.Kill();
        }
        if (points)
        {
            Cleaner.instance.Stop();
        }
        SaveLoad.Save(this);
    }

    public static void SetCameraTarget(Transform t)
    {
        playerRoom.vCam.Follow = t;
    }
}
