﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTransition : MonoBehaviour
{
    public RoomTransition nextRoom;
    public Transform entryStartPoint, entryEndPoint;
    public Transform exitStartPoint, exitEndPoint;
    public float fadeTime = 1;
    public Collider col;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        StartTransitionExit(other.GetComponentInParent<PlayerMovement>());
    }
    void StartTransitionExit(PlayerMovement pm)
    {
        Vector2 dir = exitEndPoint.position - exitStartPoint.position;
        dir.Normalize();
        //Disable player controls
        //Move player right until fade complete
        pm.SimulateInput(dir);

        //Start Fade to black
        ScreenFader.FadeToBlack(fadeTime, () => { nextRoom.StartTransitionEnter(pm); });

        //Wait on black for X seconds

    }
    void StartTransitionEnter(PlayerMovement pm)
    {
        col.enabled = false;
        //Teleport player and camera to next Room
        pm.transform.position = entryStartPoint.position;
        StartCoroutine(EndTransitionUpdate(pm));
    }
    void EndTransition(PlayerMovement pm)
    {
        //Enable player controls
        pm.StopSimulateInput();
        col.enabled = true;
    }

    IEnumerator EndTransitionUpdate(PlayerMovement pm)
    {
        //pm.SimulateInput(Vector2.zero);

        //Start Fade from black
        ScreenFader.FadeFromBlack(fadeTime / 2, 0.25f, null);

        Vector2 dir = entryEndPoint.position - entryStartPoint.position;
        dir.Normalize();
        //Move player Right until they reach point Y
        pm.SimulateInput(dir);

        while (true)
        {
            float distance = Vector3.Distance(pm.transform.position, entryEndPoint.position);
            if (distance <= 0.01f)
            {
                EndTransition(pm);
                yield break;
            }
            yield return null;
        }
    }
}
