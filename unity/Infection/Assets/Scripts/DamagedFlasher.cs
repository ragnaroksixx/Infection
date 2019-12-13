using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DamagedFlasher : MonoBehaviour
{
    public Renderer[] meshes;
    public const float flashRate = 0.2f;
    public Color flashColor = Color.white;
    private void Start()
    {
        //Flash(10);
    }
    public void Flash(float duration)
    {
        foreach (Renderer mesh in meshes)
        {
            Sequence s = DOTween.Sequence();
            Color normalColor = mesh.material.color;
            Tween toRed = mesh.material.DOColor(flashColor, flashRate / 2);
            Tween toNormal = mesh.material.DOColor(normalColor, flashRate / 2);
            mesh.DOKill();
            s.Append(toRed)
                .Append(toNormal).OnKill(
                () =>
                {
                    mesh.material.color = normalColor;
                });

            s.SetLoops((int)(duration / flashRate));
            Debug.LogWarning((int)(duration / flashRate));
            s.Play();
        }
    }
}
