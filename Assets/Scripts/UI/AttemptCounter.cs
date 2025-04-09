using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AttemptCounter : MonoBehaviour
{
    public Text attemptCount;
    
    void Start()
    {
        EventManager.StartListening("OnStartLevel", (levelResult) => UpdateCount());
    }

    
    // Update is called once per frame
    void Update()
    {
     
    }

    void UpdateCount()
    {
        string scene = SceneManager.GetActiveScene().name;
        Level level = SaveManager.GetLevelHistory(scene);
        int attempts = level.performance.attempts;
        attemptCount.text = attempts.ToString();
    }
}
