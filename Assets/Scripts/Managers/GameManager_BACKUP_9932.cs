using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : Singleton<GameManager> {
	[SerializeField]
	private GameObject splashObject;
	[SerializeField]
	private GameObject waveGenerator;
	[SerializeField]
	private GameObject terrain;
	private Collider2D terrainCollider;
	private const float _TIEMPO_PANTALLA_LOGO = 7.6f;
	private float tiempoTranscurridoMenuPrincipal;
	// Use this for initialization
	void Start () {
		this.tiempoTranscurridoMenuPrincipal = 0;
		Collider2D terrainCollider = terrain.GetComponent<Collider2D>();
	}
	
	// Update is called once per frame
	void Update () {
		if(SceneManager.GetActiveScene().name == "MenuPrincipal") {
			GestionaMenuSplash();
		}
		AplicarInteraccion ();
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

<<<<<<< HEAD
	public void ExitGame() {
		Application.Quit ();
=======
	void AplicarInteraccion () {
		AplicarRaton ();
	}

	void AplicarRaton () {

		if (Input.GetButtonDown ("Fire1")) {
			
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit2D hit;
			hit = Physics2D.GetRayIntersection(ray);

			if (hit.collider != null) {
				Debug.Log (hit.transform);

				Debug.DrawLine(ray.origin, hit.point);
				Instantiate (waveGenerator, hit.point, Quaternion.identity);
			}
		}
>>>>>>> 935607c054a3ba8de08ff5c44c405e7deb662d57
	}
}
