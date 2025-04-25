using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Progress;
using static UnityEngine.GraphicsBuffer;

[System.Serializable]
public class ElementData
{
    public string type;
    public string data;
}
public interface ILevelElement
{
    string Type();
    ElementData AsLevelData();
    void LoadFromLevelData(ElementData levelData);
    void SetInert();
}

public abstract class LevelElementBase : MonoBehaviour, ILevelElement
{
    // Shared properties
    public GameObject UIPrefab;

    public abstract string Type();
    public abstract ElementData AsLevelData();
    public abstract void LoadFromLevelData(ElementData levelData);

    // Provide a shared implementation
    public virtual void SetInert()
    {

    }

}


public class MetaLevel : MonoBehaviour
{
    private string jsonPath = string.Empty;
    public GameObject interactionObject;
    private LevelData tempLevelData;
    public LevelData workingLevelData;

    public GameObject[] elementPrefabs;

    public GameObject negativeObstaclePrefab;

    void Start()
    {
        //load level json
 
        workingLevelData = new LevelData()
        {
            world = string.Empty,
            levelId = GameManager.TargetLevel,
            waveTarget = 0,
            timeTarget = 0,
        };
    }

    // Update is called once per frame
    void Update()
    {

    }
    public LevelData PortLevelToData()
    {
        if (string.IsNullOrEmpty(workingLevelData.levelId))
        {
            workingLevelData.levelId = System.Guid.NewGuid().ToString();
            GameManager.SetTargetLevel(workingLevelData.levelId);
        }
        LevelData levelData = new LevelData()
        {
            world = workingLevelData.world,
            levelId = workingLevelData.levelId,
            name = workingLevelData.levelId,
            cameraMode = workingLevelData.cameraMode,
            size = workingLevelData.size,
            waveTarget = workingLevelData.waveTarget,
            timeTarget = workingLevelData.timeTarget,
            levelElements = GetComponentsInChildren<ILevelElement>().Select(el => el.AsLevelData()).ToArray(),
        };

        return levelData;
    }
    //solo se usa en el boton
    public void SaveLevelToJson()
    {
        tempLevelData = PortLevelToData();
        saveToFile(tempLevelData);
    }

    public void LoadLevelFromJson()
    {
        foreach (GameObject elementGo in GetComponentsInChildren<ILevelElement>().Select(el => ((MonoBehaviour)el).gameObject))
        {
            Destroy(elementGo);
        }
        InstancePrefabs(negativeObstaclePrefab);
        foreach (GameObject prefab in elementPrefabs)
        {
            InstancePrefabs(prefab);
        }
    }

    private void InstancePrefabs(GameObject prefab)
    {
        //Usar eldel level loader
        string type = prefab.GetComponent<ILevelElement>().Type();
        foreach (ElementData item in tempLevelData.levelElements.Where(el => el.type == type))
        {
            GameObject ob = Instantiate(prefab, this.transform);
            ILevelElement script = ob.GetComponent<ILevelElement>();
            script.LoadFromLevelData(item);

        }
    }

    private void saveToFile(LevelData levelData)
    {
        
        throw new System.NotImplementedException();
        jsonPath = Path.Combine(Application.persistentDataPath, "levels/" + workingLevelData.levelId + ".json");
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