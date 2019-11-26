using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Door : MonoBehaviour
{
    static List<Door> allDoors = new List<Door>();
    Vector3 closePos = new Vector3(-1, 0, 0);
    Vector3 openPos = new Vector3(-1, 1f, 0);
    float speed = 0.25f;
    private void Awake()
    {
        allDoors.Add(this);
    }
    public static void CloseAllDoors()
    {
        foreach (Door d in allDoors)
        {
            d.Close();
        }
    }
    public void Open()
    {
        transform.DOLocalMove(openPos, speed).SetDelay(0.25f);
    }

    public void Close()
    {
        transform.DOLocalMove(closePos, speed);
    }
}

