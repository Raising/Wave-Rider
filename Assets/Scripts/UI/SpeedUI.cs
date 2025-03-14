using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SpeedUI : MonoBehaviour
{
    public Text speedText;

    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale > 0)
        {

            speedText.text = "X" + Time.timeScale.ToString();
        }
        else
        {
            speedText.text = "PAUSED";
        }
    }
}
