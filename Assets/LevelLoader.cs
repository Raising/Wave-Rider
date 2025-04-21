using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    private string levelId = string.Empty;
    private MetaLevel metalevel;
    //public GameObject[] elementPrefabs;
    // Start is called before the first frame update
    void Start()
    {
        metalevel = FindObjectOfType<MetaLevel>();
        levelId = GameManager.TargetLevel;
        if (levelId == string.Empty)
        {
            levelId = "0fe419e5-5095-4be5-bc75-792f17f3aae1";
            GameManager.SetTargetLevel(levelId);
        }

        LevelData levelData = SaveManager.Instance.Data.createdLevelList.First((data) => data.levelId == levelId);
        LoadLevelFromJson(levelData);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadLevelFromJson(LevelData levelData)
    {

        //TODO set Size
        foreach (GameObject elementGo in GetComponentsInChildren<ILevelElement>().Select(el => ((MonoBehaviour)el).gameObject))
        {
            Destroy(elementGo);
        }
        foreach (GameObject prefab in metalevel.elementPrefabs)
        {
            string type = prefab.GetComponent<ILevelElement>().Type();
            foreach (ElementData item in levelData.levelElements.Where(el => el.type == type))
            {

                GameObject ob = Instantiate(prefab, this.transform);
                ILevelElement script = ob.GetComponent<ILevelElement>();
                try
                {
                    script.LoadFromLevelData(item);
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
}
