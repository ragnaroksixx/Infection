using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTransition : MonoBehaviour
{
    public RoomTransition nextRoom;
    public Transform entryStartPoint, entryEndPoint;
    public Transform exitStartPoint, exitEndPoint;
    public float fadeTime = 1;
    public Collider col;
    Room currentRoom;
    Door d;
    public bool skipToSpawn;
    // Start is called before the first frame update
    void Start()
    {
        currentRoom = GetComponentInParent<Room>();
        d = GetComponentInChildren<Door>();
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
        if (pm == null) return;
        pm.InterruptAttack();
        Vector2 dir = exitEndPoint.position - exitStartPoint.position;
        dir.Normalize();
        //Disable player controls
        //Move player right until fade complete
        if (!skipToSpawn)
            pm.SimulateInput(dir);
        currentRoom.Exit();

        //Start Fade to black
        ScreenFader.FadeToBlack(fadeTime, () =>
        {
            nextRoom.StartTransitionEnter(pm);
            if (d)
                d.Close();
        });
        if (nextRoom.d)
            nextRoom.d.Open(true);
        //Wait on black for X seconds

    }
    void StartTransitionEnter(PlayerMovement pm)
    {
        pm.InterruptAttack();
        col.enabled = false;
        //Teleport player and camera to next Room
        pm.transform.position = entryStartPoint.position;
        currentRoom.Enter();
        if (skipToSpawn)
        {
            pm.transform.position = currentRoom.SpawnPoint.position;
            ScreenFader.FadeFromBlack(fadeTime / 2, 0.25f, null);
            EndTransition(pm);
        }
        else
            StartCoroutine(EndTransitionUpdate(pm));
    }
    void EndTransition(PlayerMovement pm)
    {
        //Enable player controls
        pm.StopSimulateInput();
        col.enabled = true;
        if (d)
            d.Close();
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
            if (distance <= 0.2f)
            {
                pm.SimulateInput(Vector3.zero);
                yield return new WaitForSeconds(.5f);
                EndTransition(pm);
                yield break;
            }
            yield return null;
        }
    }
}
