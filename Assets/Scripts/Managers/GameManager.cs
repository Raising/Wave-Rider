using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum GameState {
	ready, playing, paused, win, gameOver
}
public class GameManager : MonoBehaviour {
    public static GameManager Instance = null;
    [SerializeField]
	private GameObject splashObject;
	[SerializeField]
	private EmitterSelectorButton currentButton = null;
	private GameObject waveGenerator;
	[SerializeField]
	private GameObject menuPrincipal;
	[SerializeField] private GameState gameState;

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


        // Use this for initialization
        void Start () {
		gameState = GameState.playing;
    }
	
	// Update is called once per frame
	void Update () {
		string currentScene = SceneManager.GetActiveScene ().name;
		if (currentScene.Contains ("Nivel")) {
			AplicarInteraccion ();
		} else if (currentScene == "MenuPrincipal") {
			if (InputController.CheckUserInput()) {
				GameObject pressStart = GameObject.FindGameObjectWithTag ("PressStart");
				if (pressStart.activeInHierarchy) {
					CambiaEscena ("SeleccionNivel");
				}
			}
		}
	}

	public void MenuPrincipal() {
		
	}
		
	public void CambiaEscena(string nombreEscena) {
		SceneManager.LoadScene(nombreEscena);
	}
		
	public void ExitGame() {
		Application.Quit ();
	}

	void AplicarInteraccion () {
		
		AplicarRaton ();
	}

	public void winLevel () {
		
	
		StartCoroutine (returnToLevelSelection());

	}

	IEnumerator returnToLevelSelection () {
		yield return new WaitForSeconds (AudioManager.Instance.SoundResources("Win.wav").length);
		CambiaEscena ("SeleccionNivel");
	}



	public void loseLevel() {
		
		StartCoroutine (repeatLevel());

	}

	IEnumerator repeatLevel () {
		yield return new WaitForSeconds (AudioManager.Instance.SoundResources("Loose.wav").length);
		CambiaEscena ( SceneManager.GetActiveScene ().name);
	}

	void AplicarRaton () {

		if (Input.GetButtonDown ("Fire1") ) {
			
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit2D hit;
			hit = Physics2D.GetRayIntersection(ray);

			if (hit.collider != null){
				Debug.Log (hit.collider.tag);
				if (currentButton != null && (hit.collider.tag == "Terrain" || hit.collider.tag == "Wave"))  {
					GameObject emitter = currentButton.getEmitter ();
					if (emitter != null) {
						Instantiate (emitter, hit.point, Quaternion.identity);
						currentButton.reduceAmmo ();
					} 
									}
				else if(hit.collider.tag == "EmitterSelector"){
					if (currentButton != null ){
						currentButton.deselect();
					}
					currentButton = (EmitterSelectorButton)hit.collider.GetComponent(typeof(EmitterSelectorButton));
					currentButton.select();
				}
			}
		}
	}

	private void PauseResumeGame() {
		if (gameState == GameState.paused) {
			Resume ();		
		} else if (gameState == GameState.playing) {
			Pause ();
		}
	}

	private void Pause() {
		Time.timeScale = 0;
		gameState = GameState.paused;
	}

	private void Resume() {
		Time.timeScale = 1;
		gameState = GameState.playing;
	}
}
