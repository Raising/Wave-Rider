using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveCounter : MonoBehaviour
{
    public Text wavesTextDisplay;
    public Text starWavesText;
    private Boolean isTracking = false;
    // Start is called before the first frame update
    void Start()
    {
        starWavesText.text = LevelManager.Instance.maxWavesStar.ToString();
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

    void Update()
    {
        if (isTracking)
        {
            wavesTextDisplay.text = LevelManager.GetLevelState().generatedWaves.ToString();
        }
    }
}
