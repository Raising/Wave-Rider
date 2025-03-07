using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

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
            timeStart = 0,//Time.time,
            timeEnd = 0,
            level = 0,
            generatedWaves = 0,
        };
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
        EventManager.TriggerEvent("OnEndLevel", EnumLevelResult.Win);
    }
    public static void HandleLevelFail()
    {
        EventManager.TriggerEvent("OnEndLevel", EnumLevelResult.Lose);
    }

}


public struct LevelState
{
    public float timeStart;
    public float timeEnd;
    public int level;
    public int generatedWaves;
}
