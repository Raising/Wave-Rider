using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HeaderUI : MonoBehaviour
{
    public Text levelNameText;

    void Start()
    {
        string scene = SceneManager.GetActiveScene().name;
     
        string world;
        string level;
        string[] worldName = scene.Split('_');
        if (worldName.Length == 2)
        {
            world = scene.Split('_')[0];  // e.g. "intro"
            level = scene.Split('_')[1]; // e.g. "1"
        }
        else
        {
            level = scene;
            world = "test";
        }
        levelNameText.text = world + " " + level;
    }


    // Update is called once per frame
    void Update()
    {

    }
}
