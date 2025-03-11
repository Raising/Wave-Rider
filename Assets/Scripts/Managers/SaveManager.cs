using System;
using System.Collections;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.Analytics;

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
            //DontDestroyOnLoad(gameObject);
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
    static public Level GetLevelData(string levelName)
    {
        string world = levelName.Split('_')[0];  // e.g. "intro"
        string level = levelName.Split('_')[1]; // e.g. "1"

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
            bests = new Bests()
            {
                bestTime = 999,
                bestWaves = 999,

            }
        };
    }
    public static void SaveLevel(LevelState levelState)
    {
        string world = levelState.levelName.Split('_')[0];  // e.g. "intro"
        string level = levelState.levelName.Split('_')[1]; // e.g. "1"

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
        Stars stars = Instance.Data.worlds[world].levels[level].stars;
        stars.completed = levelState.winned || stars.completed;
        stars.inTime = levelState.timeSuccess || stars.inTime;
        stars.inWaves = levelState.waveSuccess || stars.inWaves;
        stars.inAll = (levelState.timeSuccess && levelState.waveSuccess) || stars.inAll;


        // Guardar los datos en el nivel correspondiente
        Instance.Data.worlds[world].levels[level].stars = stars;
        SaveGame();

        Debug.Log($"✅ Nivel guardado: {levelState.levelName}");
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
        Debug.Log("🔄 Datos reseteados a valores predeterminados.");
    }


}