using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public enum EnumLevelResult
{
    Win,
    Lose,
}

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance = null;
    private LevelState levelState;
    public float maxTimeStar;
    public int maxWavesStar;
 
    // Start is called before the first frame update

    void Awake()
    {
        Instance = this;

        StartCoroutine(StartLevel());
        EventManager.StartListening("OnWaveCreation", (waveInfo) => levelState.generatedWaves = levelState.generatedWaves + 1);
        EventManager.StartListening("OnLevelCompletion", (waveInfo) => HandleLevelCompletion());
        EventManager.StartListening("OnLevelFail", (waveInfo) => HandleLevelFail());
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator StartLevel()
    {
        yield return new WaitForSeconds(0.5f);
        Instance.levelState = new LevelState
        {
            levelName = SceneManager.GetActiveScene().name,
            realStart = Time.time,
            timeStart = 0,//Time.time,
            timeEnd = 0,
            level = 0,
            generatedWaves = 0,
            timeSuccess = false,
            waveSuccess = false,
            winned = false,
        };
        SaveManager.IncreaseAttempts(Instance.levelState);
        EventManager.TriggerEvent("OnStartLevel");
    }

    public static LevelState GetLevelState()
    {
        return Instance.levelState;
    }
    public static void StartTimer()
    {
        if (Instance.levelState.timeStart == 0)
        {
            Instance.levelState.timeStart = Time.time;
        }
    }

    public static void HandleLevelCompletion()
    {
        Instance.levelState.timeEnd = Time.time;
        float elapsedTime = Instance.levelState.timeEnd - Instance.levelState.timeStart;
        Instance.levelState.waveSuccess = Instance.maxWavesStar >= Instance.levelState.generatedWaves;
        Instance.levelState.timeSuccess = Instance.maxTimeStar > elapsedTime;
        Instance.levelState.winned = true;
        SaveManager.SaveLevelHistory(Instance.levelState);
        EventManager.TriggerEvent("OnEndLevel", EnumLevelResult.Win);
    }
    public static void HandleLevelFail()
    {
        Instance.levelState.timeEnd = Time.time;
        Instance.levelState.waveSuccess = false;
        Instance.levelState.timeSuccess = false;
        Instance.levelState.winned = false;
        SaveManager.SaveLevelHistory(Instance.levelState);
        EventManager.TriggerEvent("OnEndLevel", EnumLevelResult.Lose);
    }

}


public struct LevelState
{
    public string levelName;
    public float realStart;
    public float timeStart;
    public float timeEnd;
    public bool winned;
    public int level;
    public int generatedWaves;
    public bool waveSuccess;
    public bool timeSuccess;
}
