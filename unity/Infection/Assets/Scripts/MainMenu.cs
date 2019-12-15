using UnityEngine;
using System.Collections;
using DG.Tweening;


public class MainMenu : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton7) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            NewGame();
        }
        else if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            Continue();
        }
    }

    public void NewGame()
    {
        SaveLoad.NewGame();
    }

    public void Continue()
    {
        SaveLoad.Continue();
    }
}

