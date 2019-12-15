using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DamagedFlasher : MonoBehaviour
{
    public Renderer[] meshes;
    Color[] colors;
    public const float flashRate = 0.2f;
    public Color flashColor = Color.white;
    private void Start()
    {
        colors = new Color[meshes.Length];
        for (int i = 0; i < meshes.Length; i++)
        {
            colors[i] = meshes[i].material.color;
        }
    }
    public void Flash(float duration)
    {
        for (int i = 0; i < meshes.Length; i++)
        {
            Renderer mesh = meshes[i];
            Sequence s = DOTween.Sequence();
            mesh.DOComplete();
            mesh.DOKill();

            Color normalColor = colors[i];
            mesh.material.color = normalColor;
            Tween toRed = mesh.material.DOColor(flashColor, flashRate / 2);
            Tween toNormal = mesh.material.DOColor(normalColor, flashRate / 2);
            s.Append(toRed)
                .Append(toNormal).OnKill(
                () =>
                {
                    mesh.material.color = normalColor;
                }).OnComplete(
                () =>
                {
                    mesh.material.color = normalColor;
                });

            s.SetLoops((int)(duration / flashRate));
            s.Play();
        }
    }
}
