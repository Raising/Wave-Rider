using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : Singleton<GameManager> {
	[SerializeField]
	private GameObject splashObject;

	private const float _TIEMPO_PANTALLA_LOGO = 7.6f;
	private float tiempoTranscurridoMenuPrincipal;
	// Use this for initialization
	void Start () {
		this.tiempoTranscurridoMenuPrincipal = 0;
	}
	
	// Update is called once per frame
	void Update () {
		/*if(SceneManager.GetActiveScene().name == "MenuPrincipal") {
			GestionaMenuSplash();
		}*/
		
	}

	public void CambiaEscena(string nombreEscena) {
		SceneManager.LoadScene(nombreEscena);
	}

	public void GestionaMenuSplash() {
		Image imagenSplash = splashObject.GetComponent<Image>();
		if(this.tiempoTranscurridoMenuPrincipal >= _TIEMPO_PANTALLA_LOGO) {
			if(imagenSplash.color.a > 0) {
				Color restandoAlpha = new Color(imagenSplash.color.r, imagenSplash.color.g, imagenSplash.color.b, imagenSplash.color.a - 0.02f);
				imagenSplash.color = restandoAlpha;
			} else {
				splashObject.SetActive(false);
			}

		} else {
			tiempoTranscurridoMenuPrincipal += Time.deltaTime;
		}
	}
}
