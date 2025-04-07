using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerData
{
    public int totalStars = 0;
    [SerializeField]
    public Unlocks unlocks = new Unlocks();

   
    [SerializeField]
    private List<WorldEntry> worldsList = new List<WorldEntry>();

    public Dictionary<string, Levels> worlds = new Dictionary<string, Levels>();

    // Convert Dictionary to List before saving
    public void PrepareForSerialization()
    {
        worldsList.Clear();
        foreach (var kvp in worlds)
        {
            kvp.Value.PrepareForSerialization();
            worldsList.Add(new WorldEntry { worldName = kvp.Key, levels = kvp.Value });
        }
    }

    // Restore Dictionary from List after loading
    public void RestoreFromSerialization()
    {
        worlds.Clear();
        foreach (var entry in worldsList)
        {
            entry.levels.RestoreFromSerialization();
            worlds[entry.worldName] = entry.levels;
        }
    }
}

[System.Serializable]
public class WorldEntry
{
    public string worldName;
    public Levels levels;
}

[System.Serializable]
public class LevelEnty
{
    public string levelName;
    public Level level;
}
[System.Serializable]
public class Unlocks
{
}
// Diccionario de niveles, clave = nombre del nivel (e.g. "level1"), valor = Level
[System.Serializable]
public class Levels
{
    public int unlockedStars = 0;
    [SerializeField]
    private List<LevelEnty> levelList = new List<LevelEnty>();
    public Dictionary<string, Level> levels = new Dictionary<string, Level>();
    public void PrepareForSerialization()
    {
        levelList.Clear();
        foreach (var kvp in levels)
        {
            levelList.Add(new LevelEnty { levelName = kvp.Key, level = kvp.Value });
        }
    }

    // Restore Dictionary from List after loading
    public void RestoreFromSerialization()
    {
        levels.Clear();
        foreach (var entry in levelList)
        {
            levels[entry.levelName] = entry.level;
        }
    }
}
[System.Serializable]
public class Level
{
    [SerializeField]
    public Stars stars = new Stars();
    [SerializeField]
    public Performance performance = new Performance();
}
[System.Serializable]
public class Stars
{
    [SerializeField]
    public bool completed = false;
    [SerializeField]
    public bool inTime = false;
    [SerializeField]
    public bool inWaves = false;
    [SerializeField]
    public bool inAll = false;
}
[System.Serializable]
public class Performance
{
    [SerializeField]
    public int attempts = 0;
    [SerializeField]
    public float totalTime = 0;
    [SerializeField]
    public int bestWaves = 99;
    [SerializeField]
    public float bestTime = 99;
}
