using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HeaderUI : MonoBehaviour
{
    public Text levelNameText;

    void Start()
    {
        string scene = SceneManager.GetActiveScene().name;
        string world = scene.Split('_')[0];
        string number = scene.Split('_')[1];
        levelNameText.text = world + " " + number;
    }


    // Update is called once per frame
    void Update()
    {

    }
}
