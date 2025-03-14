using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeCounter : MonoBehaviour
{
    public Text timeTextDisplay;
    public Text starTimeText;
    private Boolean isTracking = false;
    void Start()
    {
        starTimeText.text = LevelManager.Instance.maxTimeStar.ToString();
        EventManager.StartListening("OnEndLevel", (levelResult) => StopTracking());
        EventManager.StartListening("OnStartLevel", (levelResult) => StartTracking());
    }

    private void StopTracking()
    {
        isTracking = false;
    }

    private void StartTracking()
    {
        isTracking = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTracking)
        {
            float startTime = LevelManager.GetLevelState().timeStart;
            if (startTime == 0)
            {
                timeTextDisplay.text = "0";
            }
            else
            {
                float elapsedTime = Time.time - LevelManager.GetLevelState().timeStart;
                float seconds = Mathf.Floor(elapsedTime);
                float decimals = Mathf.Floor((elapsedTime - seconds) * 10);
                timeTextDisplay.text = seconds.ToString() + "." + decimals.ToString();
            }
        }
    }
}
