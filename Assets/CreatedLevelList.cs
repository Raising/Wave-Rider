using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatedLevelList : MonoBehaviour
{
    private int selectedLevel = 0;
    public GameObject levelMiniturePrefab;

    // Start is called before the first frame update
    void Start()
    {
        LevelData[] levels = SaveManager.Instance.Data.createdLevelList;
        foreach (LevelData level in levels)
        {
            Instantiate(levelMiniturePrefab, transform);
            levelMiniturePrefab.GetComponent<LevelMiniature>().AssignLevel(level);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
