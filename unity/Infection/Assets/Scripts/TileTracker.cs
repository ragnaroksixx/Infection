using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileTracker : MonoBehaviour
{
    public int id;
    public static List<TileUI> allTiles = new List<TileUI>();
    public static int currentTile = -1;
    private void OnTriggerEnter(Collider other)
    {
        SetTile(currentTile);
    }
    private void OnDestroy()
    {
        allTiles = new List<TileUI>();
    }
    public static void SetTile(int i)
    {
        if (currentTile == i) return;
        foreach (TileUI item in allTiles)
        {
            if (item.id == currentTile)
                item.Exit();
        }
        currentTile = i;
        foreach (TileUI item in allTiles)
        {
            item.SetVisibility(currentTile);
        }
    }
}
