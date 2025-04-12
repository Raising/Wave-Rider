using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    private string levelId = string.Empty;
    public GameObject touchableArea;

    public GameObject[] elementPrefabs;
    // Start is called before the first frame update
    void Start()
    {
        levelId = GameManager.TargetLevel;
        if (levelId == string.Empty)
        {
            levelId = "3ec35a10-98b0-494f-99ad-47807c85f7d4";
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
        Instantiate(touchableArea, this.transform);
        //TODO set Size
        foreach (GameObject elementGo in GetComponentsInChildren<ILevelElement>().Select(el => ((MonoBehaviour)el).gameObject))
        {
            Destroy(elementGo);
        }
        foreach (GameObject prefab in elementPrefabs)
        {
            string type = prefab.GetComponent<ILevelElement>().Type();
            foreach (ElementData item in levelData.levelElements.Where(el => el.type == type))
            {
                GameObject ob = Instantiate(prefab, this.transform);
                ILevelElement script = ob.GetComponent<ILevelElement>();
                script.LoadFromLevelData(item);

            }
        }
    }
}
