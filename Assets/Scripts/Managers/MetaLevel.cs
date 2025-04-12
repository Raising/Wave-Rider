using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Progress;
using static UnityEngine.GraphicsBuffer;


public class ElementData
{
    public string type;
    public object data;
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
    public string worldName = string.Empty;
    public string levelId = "";
    private string jsonPath = string.Empty;
    public GameObject interactionObject;
    public int waveTarget = 0;
    public float timeTarget = 0;
    private LevelData tempLevelData;

    public GameObject[] elementPrefabs;
    //public GameObject obstaclePrefab;
    //public GameObject blockPrefab;
    //public GameObject ballPrefab;
    //public GameObject exitPrefab;
    //public GameObject flowPrefab;
    // Start is called before the first frame update
    void Start()
    {
        //load level json

    }

    // Update is called once per frame
    void Update()
    {

    }
    public LevelData PortLevelToData()
    {
        if (string.IsNullOrEmpty(levelId))
        {
            levelId = System.Guid.NewGuid().ToString();
        }
        LevelData levelData = new LevelData()
        {
            world = worldName,
            levelId = levelId,
            name = levelId,
            waveTarget = waveTarget,
            timeTarget = timeTarget,
            levelElements = GetComponentsInChildren<ILevelElement>().Select(el => el.AsLevelData()).ToArray(),
        };

        return levelData;
    }
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
        foreach (GameObject prefab in elementPrefabs)
        {
            string type = prefab.GetComponent<ILevelElement>().Type();
            foreach (ElementData item in tempLevelData.levelElements.Where(el => el.type == type))
            {
                GameObject ob = Instantiate(prefab, this.transform);
                ILevelElement script = ob.GetComponent<ILevelElement>();
                script.LoadFromLevelData(item);

            }
        }
    }

    private void saveToFile(LevelData levelData)
    {
        jsonPath = Path.Combine(Application.persistentDataPath, "levels/" + levelId + ".json");
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