using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    private string path;
    public PlayerData Data { get; private set; }

    private void Awake()
    {
        // Implementar Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        path = Path.Combine(Application.persistentDataPath, "save.json");
        LoadGame(); // Cargar datos al iniciar el juego
    }

    static public void SaveGame()
    {
        Instance.Data.PrepareForSerialization();
        string json = JsonUtility.ToJson(Instance.Data, true);
        Instance.StartCoroutine(Instance.SaveToFile(json));
    }
    static public Level GetLevelHistory(string levelName)
    {
        string world;
        string level;
        string[] worldName = levelName.Split('_');
        if (worldName.Length == 2)
        {
            world = levelName.Split('_')[0];  // e.g. "intro"
            level = levelName.Split('_')[1]; // e.g. "1"
        }
        else
        {
            level = levelName;
            world = "test";
        }

        if (Instance.Data.worlds.ContainsKey(world))
        {
            if (Instance.Data.worlds[world].levels.ContainsKey(level))
            {
                return Instance.Data.worlds[world].levels[level];
            }
        }
        return new Level()
        {
            stars = new Stars()
            {
                completed = false,
                inAll = false,
                inTime = false,
                inWaves = false
            },
            performance = new Performance()
            {
                attempts = 0,
                totalTime = 0,
                bestTime = 999,
                bestWaves = 999,

            }
        };
    }
    public static void IncreaseAttempts(LevelState levelState)
    {
        string world;
        string level;
        string[] worldName = levelState.levelName.Split('_');
        if (worldName.Length == 2)
        {
        world = levelState.levelName.Split('_')[0];  // e.g. "intro"
        level = levelState.levelName.Split('_')[1]; // e.g. "1"
        }
        else
        {
            level = levelState.levelName;
            world = "test";
        }

        // Asegurar que el mundo existe en el diccionario
        if (!Instance.Data.worlds.ContainsKey(world))
        {
            Instance.Data.worlds[world] = new Levels();
        }

        // Asegurar que el nivel existe en el diccionario
        if (!Instance.Data.worlds[world].levels.ContainsKey(level))
        {
            Instance.Data.worlds[world].levels[level] = new Level();
        }
        Performance performance = Instance.Data.worlds[world].levels[level].performance;
        performance.attempts = performance.attempts + 1;
        Instance.Data.worlds[world].levels[level].performance = performance;
        SaveGame();
    }
    public static void SaveLevelHistory(LevelState levelState)
    {
        string world;
        string level;
        string[] worldName = levelState.levelName.Split('_');
        if (worldName.Length == 2)
        {
            world = levelState.levelName.Split('_')[0];  // e.g. "intro"
            level = levelState.levelName.Split('_')[1]; // e.g. "1"
        }
        else
        {
            level = levelState.levelName;
            world = "test";
        }

        // Asegurar que el mundo existe en el diccionario
        if (!Instance.Data.worlds.ContainsKey(world))
        {
            Instance.Data.worlds[world] = new Levels();
        }

        // Asegurar que el nivel existe en el diccionario
        if (!Instance.Data.worlds[world].levels.ContainsKey(level))
        {
            Instance.Data.worlds[world].levels[level] = new Level();
        }
        float time = levelState.timeEnd - levelState.timeStart;
        Stars stars = Instance.Data.worlds[world].levels[level].stars;
        stars.completed = levelState.winned || stars.completed;
        stars.inTime = levelState.timeSuccess || stars.inTime;
        stars.inWaves = levelState.waveSuccess || stars.inWaves;
        stars.inAll = (levelState.timeSuccess && levelState.waveSuccess) || stars.inAll;
        Performance performance = Instance.Data.worlds[world].levels[level].performance;
        //performance.attempts = performance.attempts + 1;
        performance.totalTime = performance.totalTime + (levelState.timeEnd - levelState.realStart);
        performance.bestTime = performance.bestTime < time ? performance.bestTime : time;
        performance.bestWaves = performance.bestWaves < time ? performance.bestWaves : levelState.generatedWaves;
        // Guardar los datos en el nivel correspondiente
        Instance.Data.worlds[world].levels[level].stars = stars;
        Instance.Data.worlds[world].levels[level].performance = performance;
        SaveGame();

        Debug.Log($"✅ Nivel guardado: {levelState.levelName}");
    }

    public static void SaveLevelData(LevelData levelData)
    {
        int index = -1;
        for (int i = 0; i < Instance.Data.createdLevelList.Length; i++)
        {
            if (Instance.Data.createdLevelList[i].levelId == levelData.levelId)
            {
                index = i; break;
            }
        }

        if (index > 0)
        {
            Instance.Data.createdLevelList[index] = levelData;
        }
        else
        {
            Instance.Data.createdLevelList = Instance.Data.createdLevelList.Append(levelData).ToArray();
        }
    }

    public static void DeleteLevelData(LevelData levelData)
    {
        LevelData[] newLevels = new LevelData[Instance.Data.createdLevelList.Length - 1];
        int j = 0;
        for (int i = 0; i < Instance.Data.createdLevelList.Length; i++)
        {
            if (Instance.Data.createdLevelList[i].levelId != levelData.levelId)
            {
                newLevels[j++] = Instance.Data.createdLevelList[i];
            }
        }

    }

    private IEnumerator SaveToFile(string json)
    {
        yield return new WaitForEndOfFrame();
        try
        {
            File.WriteAllText(path, json);
            Debug.Log("✅ Juego guardado automáticamente.");
        }
        catch (System.Exception e)
        {
            Debug.LogError("❌ Error al guardar: " + e.Message);
        }
    }

    public void LoadGame()
    {
        if (File.Exists(path))
        {
            try
            {
                string json = File.ReadAllText(path);
                Data = JsonUtility.FromJson<PlayerData>(json);
                Data.RestoreFromSerialization();
                //Debug.Log($"📂 Datos cargados - Nivel: {Data.nivel}, Vida: {Data.vida}, Nombre: {Data.nombre}");
            }
            catch (System.Exception e)
            {
                Debug.LogError("❌ Error al cargar: " + e.Message);
                Data = new PlayerData(); // Si hay error, usa valores por defecto
            }
        }
        else
        {
            Debug.Log("📂 No se encontró archivo de guardado. Creando nuevos datos.");
            Data = new PlayerData();
            SaveGame(); // Guardar datos iniciales
        }
    }

    static public void ResetData()
    {
        Instance.Data = new PlayerData();
        SaveGame();
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
        Debug.Log("🔄 Datos reseteados a valores predeterminados.");
    }


}