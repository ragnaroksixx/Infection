using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public static class SaveLoad
{
    public static int spawnRoom = 0;

    static SaveLoad()
    {
        Load();
    }
    public static void Load()
    {
        PlayerPrefs.DeleteAll();
        spawnRoom = PlayerPrefs.GetInt("spawnRoom", 0);
        spawnRoom = 99;
    }
    public static void Save(Room r)
    {
        PlayerPrefs.SetInt("spawnRoom", r.id);
        PlayerPrefs.Save();
    }
    public static void UnlockDoor(Door d)
    {
        PlayerPrefs.SetInt("door" + d.lockId, 1);
        PlayerPrefs.Save();
    }
    public static bool IsUnLocked(Door d)
    {
        return PlayerPrefs.HasKey("door" + d.lockId);
    }
    public static void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public static void Collect(Collectible c)
    {
        PlayerPrefs.SetInt(c.idTag, 0);
        PlayerPrefs.Save();
    }
    public static bool HasCollectible(string c)
    {
        return PlayerPrefs.HasKey(c);
    }
    public static int GetMaxHP()
    {
        return PlayerPrefs.GetInt("hp", 3);
    }
    public static void IncreaseMaxHP()
    {
        PlayerPrefs.SetInt("hp", GetMaxHP() + 1);
    }
}
