using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class GameManager : Singleton<GameManager> {
	[SerializeField]
	private GameObject splashObject;
	[SerializeField]
	private EmitterSelectorButton currentButton = null;
	private GameObject waveGenerator;

	private const float _TIEMPO_PANTALLA_LOGO = 7.6f;
	private float tiempoTranscurridoMenuPrincipal;
	// Use this for initialization
	void Start () {
		this.tiempoTranscurridoMenuPrincipal = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(SceneManager.GetActiveScene().name.Contains("Nivel")) {
			AplicarInteraccion();
		}
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
		
		MusicManager.Instance.playSound ("Win.wav");
		StartCoroutine (returnToLevelSelection());

	}

	IEnumerator returnToLevelSelection () {
		yield return new  WaitForSeconds (2);
		CambiaEscena ("SeleccionNivel");
	}



	public void loseLevel() {
		MusicManager.Instance.playSound ("Loose.wav");
		StartCoroutine (repeatLevel());

	}

	IEnumerator repeatLevel () {
		yield return new WaitForSeconds (2);
		CambiaEscena ("SeleccionNivel");
	}

	void AplicarRaton () {

		if (Input.GetButtonDown ("Fire1")) {
			
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
