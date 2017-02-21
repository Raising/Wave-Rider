using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour {
    public GameObject gameManager;
    public GameObject musicManager;
    public GameObject sceneController;
    public GameObject adsController;

    private void Awake() {
        if(GameManager.Instance == null) {
            Instantiate(gameManager);
        }

        if (AudioManager.Instance == null) {
            Instantiate(musicManager);
        }

        if (SceneController.Instance == null) {
            Instantiate(sceneController);
        }

        if (AdsController.Instance == null) {
            Instantiate(adsController);
        }
    }
}
