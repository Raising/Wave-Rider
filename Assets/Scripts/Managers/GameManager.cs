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
	}
}
