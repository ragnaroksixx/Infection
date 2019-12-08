using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class Button : MonoBehaviour
{
    public Door door;
    private void Start()
    {
        OnButtonHit();
    }
    public void OnButtonHit()
    {
        //Time=0
        //move target to door
        //wait
        //move target back
        //Time=1
        StartCoroutine(Sequence());
    }

    IEnumerator Sequence()
    {
        yield return new WaitForSecondsRealtime(2);
        //Time.timeScale = 0;
        door.VCam.Priority = 100;
        yield return new WaitForSecondsRealtime(1);
        door.Unlock();
        door.Open();
        yield return new WaitForSecondsRealtime(1);
        door.VCam.Priority = -100;
        yield return new WaitForSecondsRealtime(1);
        //Time.timeScale = 1;
    }
}
