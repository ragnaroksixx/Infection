using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ScreenFader : MonoBehaviour
{
    public Image image;
    static ScreenFader instance;
    private void Awake()
    {
        instance = this;
    }
    public static void FadeToBlack(float duration, Action onComplete)
    {
        instance.image.DOFade(1, duration).OnComplete(() =>
         {
             onComplete?.Invoke();
         });
    }
    public static void FadeFromBlack(float duration, float delay, Action onComplete)
    {
        instance.image.DOFade(0, duration).SetDelay(delay).OnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }
}
