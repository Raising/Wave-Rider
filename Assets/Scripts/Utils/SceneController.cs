using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

public class SceneController : Singleton<SceneController> {

	/// <summary>
	/// //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
	/// </summary>
	void OnEnable() { //TODO TRASLADAR TODO ESTO A SCENECONTROLLER
		SceneManager.sceneLoaded += SceneController.Instance.OnLevelFinishedLoading;
	}

	/// <summary>
	//Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
	/// </summary>
	void OnDisable() { //TODO TRASLADAR TODO ESTO A SCENECONTROLLER
		SceneManager.sceneLoaded -= SceneController.Instance.OnLevelFinishedLoading;
	}

	/// <summary>
	/// Se ejecuta una vez se ha finalizado de cargar un nivel
	/// </summary>
	/// <param name="scene">Escena.</param>
	/// <param name="mode">Modo de carga de la escena <value>4: Single</value>.</param>
	void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) { //TODO TRASLADAR TODO ESTO A SCENECONTROLLER
		Debug.Log("Level Loaded");
		Debug.Log(scene.name);
		Debug.Log(mode);

		string sceneName = scene.name;
		Debug.Log (sceneName);

		if (sceneName == "MenuPrincipal") { //TODO TRANQUILIDAD, ESTO SE PLANTEARA DE OTRA FORMA
			StartCoroutine (AudioManager.Instance.playMainMenuMusic ());
		} else if(sceneName.Contains("Nivel_")) {
			AudioManager.Instance.playMusic ("NIVEL GENERICO NUEVO");
		}

		Analytics.CustomEvent ("Escenas Visitadas", new Dictionary<string, object> { { "nombre", sceneName } }); // ENVIA INFORMACION PERSONALIZADA PARA LAS ESTADISTICAS
		Analytics.Transaction("12345abcde", 0.99m, "USD", null, null); // ENVIA ESTADISTICAS DE COMPRAS EN APP
		int birthYear = 2014;
		Analytics.SetUserBirthYear(birthYear);

        AdsController.Instance.CheckAdCondition(sceneName);
	}

    public bool IsSceneLevel() {
        if(SceneManager.GetActiveScene().name.Contains("Nivel_")) {
            return true;
        } else {
            return false;
        }
    }
}
