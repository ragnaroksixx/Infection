﻿using UnityEngine;
using System.Collections;
using DG.Tweening;

public class PauseMenu : MonoBehaviour
{
    public CanvasGroup pauseMenu;
    bool isPaused = false;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            TooglePause();
        }
    }

    public void TooglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
        pauseMenu.DOFade(isPaused ? 1 : 0, 0.5f).SetUpdate(true);
    }
}