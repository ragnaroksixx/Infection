using UnityEngine;
using System.Collections;
using Cinemachine;

public class Room : MonoBehaviour
{
    public const int focusPriority = 1;
    public const int unFocusPriority = 0;
    public CinemachineVirtualCamera vCam;
    public static Room playerRoom;
    public bool isFirstRoom = false;
    private void Awake()
    {
        if (isFirstRoom)
            Enter();
    }
    public void Enter()
    {
        if (playerRoom)
        {
            vCam.Follow = playerRoom.vCam.Follow;
        }
        vCam.Priority = focusPriority;
        playerRoom = this;
    }

    public void Exit()
    {
        vCam.Priority = unFocusPriority;
    }

    public static void SetCameraTarget(Transform t)
    {
        playerRoom.vCam.Follow = t;
    }
}
