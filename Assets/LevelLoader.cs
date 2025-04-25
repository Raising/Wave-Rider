using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    private string levelId = string.Empty;
    private MetaLevel metalevel;
    public bool editMode = false;
    public Boolean newLevel = false;

    //public GameObject[] elementPrefabs;
    // Start is called before the first frame update
    void Start()
    {
        metalevel = FindObjectOfType<MetaLevel>();
        levelId = GameManager.TargetLevel;
        //levelId = "0fe419e5-5095-4be5-bc75-792f17f3aae1";
        if (levelId == string.Empty)
        {
            CreateNewLevel();
        }
        else
        {
            LevelData levelData = SaveManager.Instance.Data.createdLevelList.First((data) => data.levelId == levelId);
            LoadLevelFromJson(levelData);
        }
        FindObjectOfType<Camera>().orthographicSize = metalevel.workingLevelData.size;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CreateNewLevel()
    {
        levelId = System.Guid.NewGuid().ToString();
        newLevel = true;
        GameManager.SetTargetLevel(levelId);
        metalevel.workingLevelData.levelId = levelId;
        GameObject ob = Instantiate(metalevel.negativeObstaclePrefab, this.transform);
        float size = metalevel.workingLevelData.size;
        ob.GetComponent<NegativeObstacle>().SetSize(size);

        PolygonCollider2D polygonCollider = ob.GetComponent<PolygonCollider2D>();
        float canvasSize = size * 6 / 7;
        polygonCollider.points = new Vector2[] {
            new Vector2(-canvasSize *1.5f, canvasSize ),
            new Vector2(-canvasSize *1.5f, -canvasSize ),
            new Vector2(canvasSize *1.5f, -canvasSize ),
            new Vector2(canvasSize *1.5f,canvasSize)
        };

    }

    public void LoadLevelFromJson(LevelData levelData)
    {
        //TODO set Size
        metalevel.workingLevelData = levelData;

        foreach (GameObject elementGo in GetComponentsInChildren<ILevelElement>().Select(el => ((MonoBehaviour)el).gameObject))
        {
            Destroy(elementGo);
        }
        InstancePrefabs(levelData, metalevel.negativeObstaclePrefab);
        foreach (GameObject prefab in metalevel.elementPrefabs)
        {
            InstancePrefabs(levelData, prefab);
        }
    }

    private void InstancePrefabs(LevelData levelData, GameObject prefab)
    {
        string type = prefab.GetComponent<ILevelElement>().Type();
        foreach (ElementData item in levelData.levelElements.Where(el => el.type == type))
        {

            GameObject ob = Instantiate(prefab, this.transform);
            ILevelElement script = ob.GetComponent<ILevelElement>();
            try
            {
                script.LoadFromLevelData(item);
                if (editMode)
                {
                    script.SetInert();
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"❌ Error al cargar el item de tipo '{type}': {ex.Message}");

                // Opcional: mostrar el stacktrace
                Debug.LogException(ex);

                // Opcional: log del contenido original en formato JSON
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(item, Newtonsoft.Json.Formatting.Indented);
                Debug.Log($"📦 Datos originales del item:\n{json}");
            }

        }
    }
}
