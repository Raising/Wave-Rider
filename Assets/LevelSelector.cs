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
    public Text starAmount;
    // Start is called before the first frame update
    void Start()
    {
        text.text = scene.Split('_')[1];
        button.onClick.AddListener(() => GameManager.Instance.LoadScene(scene));
        Level level = SaveManager.GetLevelData(scene);
        starAmount.text = ((level.stars.inTime ? 1 : 0) + (level.stars.inWaves ? 1 : 0) + (level.stars.completed ? 1 : 0) + (level.stars.inAll ? 1 : 0)).ToString();

        Debug.Log("RELOADsTARS");
    }

    // Update is called once per frame
    void Update()
    {

    }

}
