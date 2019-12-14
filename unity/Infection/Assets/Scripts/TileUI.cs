using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using Sirenix.OdinInspector;
public class TileUI : MonoBehaviour
{
    public RectTransform map;
    RectTransform cacheParent;
    RectTransform mapParent;
    public int id = -1;
    Vector3 cachePositon;
    public bool IsMiniMap;
    private void Awake()
    {
        TileTracker.allTiles.Add(this);
        if (IsMiniMap)
        {
            mapParent = map.parent as RectTransform;
            cacheParent = transform.parent as RectTransform;
            cachePositon = (transform as RectTransform).anchoredPosition;
        }
    }

    public void SetVisibility(int currentID)
    {
        gameObject.SetActive(id == currentID);
        if (!IsMiniMap) return;
        if (id != currentID)
        {

        }
        else
        {
            transform.SetParent(mapParent);
            map.transform.SetParent(transform);
            (transform as RectTransform).anchoredPosition = Vector3.zero;

        }
    }
    public void Exit()
    {
        if (!IsMiniMap) return;
        map.SetParent(mapParent);
        transform.SetParent(cacheParent);
        (transform as RectTransform).anchoredPosition = cachePositon;
        map.anchoredPosition = Vector3.zero;
    }

}

