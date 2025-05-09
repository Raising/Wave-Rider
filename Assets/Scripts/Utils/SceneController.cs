﻿using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

public class SceneController : MonoBehaviour {
    public static SceneController Instance = null;

    void Awake() {
        //Check if instance already exists
        if (Instance == null)

            //if not, set instance to this
            Instance = this;

        //If instance already exists and it's not this:
        else if (Instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }


    /// <summary>
    /// //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
    /// </summary>
    void OnEnable() {
		SceneManager.sceneLoaded += OnLevelFinishedLoading;
	}

	/// <summary>
	//Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
	/// </summary>
	void OnDisable() { 
		SceneManager.sceneLoaded -= OnLevelFinishedLoading;
	}

	/// <summary>
	/// Se ejecuta una vez se ha finalizado de cargar un nivel
	/// </summary>
	/// <param name="scene">Escena.</param>
	/// <param name="mode">Modo de carga de la escena <value>4: Single</value>.</param>
	void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) { //TODO TRASLADAR TODO ESTO A SCENECONTROLLER
        string sceneName = scene.name;

		if (sceneName == "MenuPrincipal") { //TODO TRANQUILIDAD, ESTO SE PLANTEARA DE OTRA FORMA
			StartCoroutine (AudioManager.Instance.playMainMenuMusic ());
		} else if(IsSceneLevel()) {
            AudioManager.Instance.playMusic ("NIVEL GENERICO NUEVO");
		}

		Analytics.CustomEvent ("Escenas Visitadas", new Dictionary<string, object> { { "nombre", sceneName } }); // ENVIA INFORMACION PERSONALIZADA PARA LAS ESTADISTICAS
		Analytics.Transaction("12345abcde", 0.99m, "USD", null, null); // ENVIA ESTADISTICAS DE COMPRAS EN APP
		int birthYear = 2014;
		Analytics.SetUserBirthYear(birthYear);
        GameManager.ResetLevelVariables();
#if UNITY_ADS
        AdsController.Instance.CheckAdCondition(sceneName);
#endif
    }

    public bool IsSceneLevel() {
        if(SceneManager.GetActiveScene().name.Contains("Level_")) {
            return true;
        } else {
            return false;
        }
    }
}
