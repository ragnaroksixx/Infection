using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class CollectibleMenu : MonoBehaviour
{
    public CanvasGroup menu;
    public static CollectibleMenu instance;
    public Image image;
    public bool isOpen;
    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (isOpen && Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            SetVis(false, null);
        }
    }

    public void SetVis(bool val, Sprite s)
    {
        isOpen = val;
        image.sprite = s;
        Time.timeScale = isOpen ? 0 : 1;
        menu.DOFade(isOpen ? 1 : 0, 0.5f).SetUpdate(true);
    }
}

