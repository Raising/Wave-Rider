using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MetaLevel : MonoBehaviour
{
    public string worldName = string.Empty;
    public int levelId = 0;
    private string jsonPath = string.Empty;
    public GameObject interactionObject;

    private LevelData tempLevelData;

    public GameObject obstaclePrefab;
    public GameObject blockPrefab;
    public GameObject ballPrefab;
    public GameObject exitPrefab;
    public GameObject flowPrefab;
    // Start is called before the first frame update
    void Start()
    {
        //load level json

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SaveLevelToJson()
    {
        LevelData levelData = new LevelData();
        ObstacleCollider[] childrenWithObstacleCollider = GetComponentsInChildren<ObstacleCollider>();
        WaterBlock[] childrenWithWaterBlock = GetComponentsInChildren<WaterBlock>();

        levelData.obstacleList = GetComponentsInChildren<ObstacleCollider>().Select(obc => obc.AsLevelData()).ToArray();
        levelData.blockList = GetComponentsInChildren<WaterBlock>().Select(obc => obc.AsLevelData()).ToArray();
        levelData.exitList = GetComponentsInChildren<ExitBeacon>().Select(obc => obc.AsLevelData()).ToArray();
        levelData.flowList = GetComponentsInChildren<WaveFlow>().Select(obc => obc.AsLevelData()).ToArray();
        levelData.ballList = GetComponentsInChildren<nutShell>().Select(obc => obc.AsLevelData()).ToArray();

        saveToFile(levelData);
        tempLevelData = levelData;
    }

    public void LoadLevelFromJson()
    {
        foreach (var item in tempLevelData.obstacleList)
        {
            Instantiate(obstaclePrefab, this.transform);
            ObstacleCollider obsC = obstaclePrefab.GetComponent<ObstacleCollider>();
            obsC.LoadFromLevelData(item);
        }
        foreach (var item in tempLevelData.blockList)
        {
            Instantiate(blockPrefab, this.transform);
            WaterBlock waterBlock = obstaclePrefab.GetComponent<WaterBlock>();
            waterBlock.LoadFromLevelData(item);
        }
        foreach (var item in tempLevelData.exitList)
        {
            Instantiate(exitPrefab, this.transform);
            ExitBeacon exitBeacon = exitPrefab.GetComponent<ExitBeacon>();
            exitBeacon.LoadFromLevelData(item);
        }
        foreach (var item in tempLevelData.flowList)
        {
            Instantiate(flowPrefab, this.transform);
            WaveFlow waveFlow = flowPrefab.GetComponent<WaveFlow>();
            waveFlow.LoadFromLevelData(item);
        }
        foreach (var item in tempLevelData.ballList)
        {
            Instantiate(ballPrefab, this.transform);
            nutShell ball = obstaclePrefab.GetComponent<nutShell>();
            ball.LoadFromLevelData(item);
        }

    }

    private void saveToFile(LevelData levelData)
    {
        jsonPath = Path.Combine(Application.persistentDataPath, "levels/" + worldName + "/" + levelId + ".json");
        string json = JsonUtility.ToJson(levelData, true);
        File.WriteAllText(jsonPath, json);
     
    }

    private IEnumerator SaveToFile(string json)
    {
        yield return new WaitForEndOfFrame();
        try
        {
            File.WriteAllText(jsonPath, json);
            Debug.Log("✅ Juego guardado automáticamente.");
        }
        catch (System.Exception e)
        {
            Debug.LogError("❌ Error al guardar: " + e.Message);
        }
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(MetaLevel))]
public class MetaLevelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();  // Draws the default inspector

        MetaLevel myComponent = (MetaLevel)target;

        // Create a button in the inspector
        if (GUILayout.Button("Save Level"))
        {
            myComponent.SaveLevelToJson();  // Triggers the function when button is pressed
        }
        // Create a button in the inspector
        if (GUILayout.Button("Load Level"))
        {
            myComponent.LoadLevelFromJson();  // Triggers the function when button is pressed
        }

    }
}
#endif