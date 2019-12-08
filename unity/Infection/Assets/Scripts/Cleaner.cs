using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Cleaner : MonoBehaviour
{
    public Transform head;
    List<Transform> path;
    public float speed;


    int start = -1, end = 0;

    public Transform StartPoint { get => path[start]; }
    public Transform EndPoint { get => path[end]; }

    float totalTime, timetrack;

    public float NormalTime { get => timetrack / totalTime; }
    public List<Pose> posBuffer;
    public int bufferSize = 1000;
    Rigidbody rBody;
    public static Cleaner instance;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        rBody = head.GetComponent<Rigidbody>();

    }
    private void Update()
    {
        if (NormalTime >= 1)
            NextPath();
        timetrack += Time.deltaTime * speed;
        Vector3 nextHeadPos = GetBezierPosition(NormalTime, StartPoint, EndPoint);
        nextHeadPos.z = 0;
        head.transform.right = -head.transform.position + nextHeadPos;
        rBody.MovePosition(nextHeadPos);
        AddToBuffer();
    }

    void AddToBuffer()
    {
        Pose p = new Pose(head.transform.position, head.transform.rotation);
        posBuffer.Insert(0, p);
        posBuffer.RemoveAt(posBuffer.Count - 1);
    }
    public void NextPath()
    {
        start = end;
        end++;
        if (end >= path.Count)
            end = 0;

        float t = 0;
        float dist = 0;
        Vector3 pos = StartPoint.position;
        Vector3 lastPos = StartPoint.position;
        do
        {
            t += 0.01f;
            lastPos = pos;
            pos = GetBezierPosition(t, StartPoint, EndPoint);
            pos.z = 0;
            Debug.DrawLine(lastPos, pos, Color.green, 2);
            Vector3 delta = pos - lastPos;
            dist += delta.magnitude;
        } while (t < 1);

        float time = 0;
        time = dist / speed;

        totalTime = time;
        timetrack = 0;
        head.position = StartPoint.position;

    }
    public bool CloseNough(Vector3 p1, Vector3 p2)
    {
        p1.z = 0;
        p2.z = 0;

        return (p1 - p2).magnitude < 0.1f;
    }
    public float scale = 1;
    // parameter t ranges from 0f to 1f
    public Vector3 GetBezierPosition(float t, Transform sPoint, Transform ePoint)
    {
        Vector3 p0 = sPoint.position;
        Vector3 p1 = p0 + (sPoint.up * scale);
        Vector3 p3 = ePoint.position;
        Vector3 p2 = p3 - (ePoint.up * scale);

        // here is where the magic happens!
        return Mathf.Pow(1f - t, 3f) * p0 + 3f * Mathf.Pow(1f - t, 2f) * t * p1 + 3f * (1f - t) * Mathf.Pow(t, 2f) * p2 + Mathf.Pow(t, 3f) * p3;
    }
    public void Stop()
    {
        gameObject.SetActive(false);
    }

    public void Start(CleanPoints c)
    {
        posBuffer = new List<Pose>();

        path = new List<Transform>(c.GetComponentsInChildren<Transform>());
        path.Remove(c.transform);
        start = -1;
        end = 0;
        NextPath();
        for (int i = 0; i < bufferSize; i++)
        {
            posBuffer.Add(new Pose(head.transform.position, head.transform.rotation));
        }
        gameObject.SetActive(true);
    }
}

