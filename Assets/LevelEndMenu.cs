using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class LevelEndMenu : MonoBehaviour
{
    public GameObject GameObject;
    public Shapes.Polygon winStar;
    public Shapes.Polygon timeStar;
    public Shapes.Polygon waveStar;
    public Text text;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.StartListening("OnEndLevel", (levelResult) => ShowEndLevelMenu((EnumLevelResult)levelResult));
        EventManager.StartListening("OnStartLevel", (levelResult) => HideEndLevelMenu());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void ShowEndLevelMenu(EnumLevelResult levelResult)
    {
        transform.SetPositionAndRotation(Vector3.zero, transform.rotation);

        if (levelResult == EnumLevelResult.Win)
        {
            LevelState levelstate = LevelManager.GetLevelState();
            text.text = "WIN!!";
            float elapsedTime = Time.time - levelstate.timeStart;
            bool waveSuccess = LevelManager.Instance.maxWavesStar >= levelstate.generatedWaves;
            bool timeSuccess = LevelManager.Instance.maxTimeStar > elapsedTime;
            if (waveSuccess)
            {
                waveStar.FillColorEnd = Color.yellow;
                waveStar.FillColorStart = Color.green;
            }
            else
            {
                waveStar.FillColorEnd = Color.gray;
                waveStar.FillColorStart = Color.gray;
            }
            if (timeSuccess)
            {
                timeStar.FillColorEnd = Color.yellow;
                timeStar.FillColorStart = Color.green;
            }
            else
            {
                timeStar.FillColorEnd = Color.gray;
                timeStar.FillColorStart = Color.gray;
            }

            winStar.FillColorEnd = Color.yellow;
            winStar.FillColorStart = Color.green;
        }
        else
        {
            text.text = "LOSE!!";
            waveStar.FillColorEnd = Color.gray;
            waveStar.FillColorStart = Color.gray;

            timeStar.FillColorEnd = Color.gray;
            timeStar.FillColorStart = Color.gray;

            winStar.FillColorEnd = Color.gray;
            winStar.FillColorStart = Color.gray;
        }
    }

    private void HideEndLevelMenu()
    {
        transform.SetPositionAndRotation(Vector3.up * 800, transform.rotation);
    }
}
