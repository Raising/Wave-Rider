using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : Singleton<GameManager> {
	[SerializeField]
	private GameObject splashObject;
	[SerializeField]
	private EmitterSelectorButton currentButton = null;
	private GameObject waveGenerator;

	[SerializeField]
	private GameObject menuPrincipal;

	private const float _TIEMPO_PANTALLA_LOGO = 7.6f;
	private float tiempoTranscurridoMenuPrincipal;
	// Use this for initialization
	void Start () {
		this.tiempoTranscurridoMenuPrincipal = 0;
	}
	
	// Update is called once per frame
	void Update () {
		string currentScene = SceneManager.GetActiveScene ().name;
		if (currentScene.Contains ("Nivel")) {
			AplicarInteraccion ();
		} else if (currentScene == "MenuPrincipal") {
			if (Input.GetButtonDown ("Fire1")) {
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
		Debug.Log ("click");
		AplicarRaton ();
	}

	public void winLevel () {
		Debug.Log ("WINNN");
	}

	void AplicarRaton () {

		if (Input.GetButtonDown ("Fire1")) {
			Debug.Log ("click");
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
}
