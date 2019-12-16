using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public static class SaveLoad
{
    public static int spawnRoom = 0;

    public static bool hasGrab { get; set; }
    public static bool hasClaw { get; set; }
    public static bool hasCorruptAbility { get; set; }

    static SaveLoad()
    {
        Load();
    }
    public static void Load()
    {
        spawnRoom = PlayerPrefs.GetInt("spawnRoom", 1);
        //spawnRoom = 3;
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
        Load();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public static void Collect(Collectible c)
    {
        PlayerPrefs.SetInt(c.idTag, 0);
        PlayerPrefs.Save();
        UpdateCollectibles();
    }

    public static int NumAirJumps()
    {
        return PlayerPrefs.HasKey("double jump") || hasAll ? 1 : 0;
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
    static bool hasAll = false;
    public static void UpdateCollectibles()
    {
        hasGrab = HasCollectible("grab") || hasAll;
        hasClaw = HasCollectible("claw") || hasAll;
        hasCorruptAbility = HasCollectible("corrupt") || hasAll;
        if (PlayerMovement.instance)
            PlayerMovement.instance.numAirJumps = SaveLoad.NumAirJumps();
    }

    public static void NewGame()
    {
        StarterTips.isNewGame = true;
        PlayerPrefs.DeleteAll();
        Load();
        UpdateCollectibles();
        SceneManager.LoadScene(1);
    }

    public static void Continue()
    {
        SceneManager.LoadScene(2);
    }
    public static void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
