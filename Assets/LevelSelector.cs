using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    public string scene;
    public Button button;
    public Text text;
    public Shapes.Polygon winStar;
    public Shapes.Polygon timeStar;
    public Shapes.Polygon waveStar;
    //public Text starAmount;
    // Start is called before the first frame update
    void Start()
    {
        text.text = scene.Split('_')[1];
        button.onClick.AddListener(() => LoadScene(scene));
        Level level = SaveManager.GetLevelData(scene);
        //starAmount.text = ((level.stars.inTime ? 1 : 0) + (level.stars.inWaves ? 1 : 0) + (level.stars.completed ? 1 : 0) + (level.stars.inAll ? 1 : 0)).ToString();
        winStar.FillColorEnd = level.stars.completed ? Color.yellow : Color.gray;
        winStar.FillColorStart = level.stars.completed ? Color.yellow : Color.gray;
        timeStar.FillColorEnd = level.stars.inTime ? Color.yellow : Color.gray;
        timeStar.FillColorStart = level.stars.inTime ? Color.yellow : Color.gray;
        waveStar.FillColorEnd = level.stars.inWaves? Color.yellow :  Color.gray;
        waveStar.FillColorStart = level.stars.inWaves ? Color.yellow : Color.gray;
    
    }

    void LoadScene(string scene)
    {
        GameManager.Instance.LoadScene(scene);
    }
    // Update is called once per frame
    void Update()
    {

    }

}
