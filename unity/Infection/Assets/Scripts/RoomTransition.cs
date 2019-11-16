using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTransition : MonoBehaviour
{
    public RoomTransition nextRoom;
    public Transform startPoint, endPoint;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void StartTransition()
    {
        //Disable player controls
        //Start Fade to black
        //Move player right until fade complete
        //Wait on black for X seconds
        //Teleport player and camera to next Room
        //Start Fade from Black
        //Move player Right until they reach point Y
        //Enable player controls
    }
}
