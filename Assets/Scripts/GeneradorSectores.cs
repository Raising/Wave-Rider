using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class GeneradorSectores : MonoBehaviour {
	[SerializeField]
	private int numeroOndas;
	[SerializeField]
	private float tiempoEntreOnda;
	[SerializeField]
	private float delayInicial;

	[SerializeField]
	private float fuerzaImpulso = 3;
	[SerializeField]
	private float traslationSpeed = 1f;
	[SerializeField]
	private float proporcionDistanciaTamanio = 0.14f;
	[SerializeField]
	private float remainingAliveTime = 5;

	private AudioSource sound;
	private float instanteCreacionGeneradorSectores;
	private float instanteUltimaOndaLanzada;

	private int ondasLanzadas;

	public GameObject waveSectorPrefab;
	public float numeroFragmentoPorOnda = 4;

	public int NumeroOndasRestantes {
		get {
			return this.numeroOndas;
		}
	}

	// Use this for initialization
	void Start () {
		this.instanteCreacionGeneradorSectores = 0;
		this.instanteUltimaOndaLanzada = 0;
		this.ondasLanzadas = 0;
		sound = GetComponent<AudioSource> ();
	}

	// Update is called once per frame
	void Update () {
		if (QuedanOndasPorEmitir ()) {
			if (NingunaOndaHaSidoLanzada ()) {
				GeneraPrimeraOndaTrasDelay ();
			} else {
				LanzarOndaTrasFrecuencia ();
			}
		} else {
			transform.localScale -= new Vector3 (Time.deltaTime*0.05f, Time.deltaTime*0.05f, 0);

			if (transform.localScale.x < 0) {
				Destroy (gameObject);
			}
		}
	}

	private bool QuedanOndasPorEmitir() {
		return this.ondasLanzadas < this.numeroOndas;
	}

	private bool NingunaOndaHaSidoLanzada() {
		return this.ondasLanzadas == 0;
	} 

	private void GeneraPrimeraOndaTrasDelay() {
		if((Time.time - this.instanteCreacionGeneradorSectores) >= this.delayInicial) {
			emitirOnda();
		}
	}

	private void LanzarOndaTrasFrecuencia() {
		if((Time.time - this.instanteUltimaOndaLanzada >= this.tiempoEntreOnda)) {
			emitirOnda();
		}
	}

	void emitirOnda (){

		sound.Play ();

		GameObject waveSector = null;
		WaveSector interfaz = null;

		for (int i = 0; i < numeroFragmentoPorOnda; i++) {

			float angle = i * Mathf.PI * 2 / numeroFragmentoPorOnda * Mathf.Rad2Deg;
	
			GameObject newWaveSector = Instantiate(waveSectorPrefab,transform.position,  Quaternion.Euler(0, 0, angle));
			interfaz = newWaveSector.GetComponent<WaveSector> ();
			interfaz.constructor (0,Mathf.PI / numeroFragmentoPorOnda,traslationSpeed,remainingAliveTime);
		}

		this.ondasLanzadas++;
		this.instanteUltimaOndaLanzada = Time.time;
	}
}
