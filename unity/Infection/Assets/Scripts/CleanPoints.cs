using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;




public class CleanPoints : MonoBehaviour
{
    public Cleaner c;
    public bool debugDraw;
    private void OnDrawGizmos()
    {
        if (!debugDraw) return;
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform t1 = transform.GetChild(i);
            int next = i + 1;
            if (next >= transform.childCount)
                next = 0;
            Transform t2 = transform.GetChild(next);
            float t = 0;
            Vector3 pos = t1.position;
            Vector3 lastPos = t1.position;
            do
            {
                t += 0.01f;
                lastPos = pos;
                pos = c.GetBezierPosition(t, t1, t2);
                pos.z = 0;
                Debug.DrawLine(lastPos, pos, Color.green);

            } while (t < 1);
        }
    }

}

