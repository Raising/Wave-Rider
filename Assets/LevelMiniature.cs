using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelMiniature : MonoBehaviour
{
    public Text text;

    private LevelData levelData;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AssignLevel(LevelData levelData)
    {
        this.levelData = levelData;
        text.text = levelData.name;
    }
    
}
