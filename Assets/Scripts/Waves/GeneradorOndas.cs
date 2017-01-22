using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradorOndas : MonoBehaviour {
	[SerializeField]
	private int numeroOndas;
	[SerializeField]
	private float tiempoEntreOnda;
	[SerializeField]
	private float delayInicial;

	[SerializeField]
	private float fuerzaImpulso = 3;
	[SerializeField]
	private float velocidad = 1f;
	[SerializeField]
	private float proporcionDistanciaTamanio = 0.14f;
	[SerializeField]
	private float tiempoDisipacionDistancia = 5;

	[SerializeField]
	private string sound = "ONDA3";

	private float instanteCreacionGeneradorOndas;
	private float instanteUltimaOndaLanzada;

	private int ondasLanzadas;

	public GameObject waveFragmentPrefab;
	public float numeroFragmentoPorOnda = 10;

	public int NumeroOndasRestantes {
		get {
			return this.numeroOndas;
		}
	}

	// Use this for initialization
	void Start () {
		this.instanteCreacionGeneradorOndas = 0;
		this.instanteUltimaOndaLanzada = 0;
		this.ondasLanzadas = 0;
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
		if((Time.time - this.instanteCreacionGeneradorOndas) >= this.delayInicial) {
			emitirOnda();
		}
	}

	private void LanzarOndaTrasFrecuencia() {
		if((Time.time - this.instanteUltimaOndaLanzada >= this.tiempoEntreOnda)) {
			emitirOnda();
		}
	}

	void emitirOnda (){
		MusicManager.Instance.playSound (sound + ".wav");
		GameObject primerWaveFragment = null;
		WaveFragment primerInterfaz = null;
		GameObject previoWaveFragment = null;
		WaveFragment previoInterfaz = null;
		GameObject waveFragment = null;
		WaveFragment interfaz = null;

		for (int i = 0; i < numeroFragmentoPorOnda; i++) {
			
           	float angle = i * Mathf.PI * 2 / numeroFragmentoPorOnda;
	       	waveFragment = Instantiate(waveFragmentPrefab, gameObject.transform.position, Quaternion.identity);
		   	
			interfaz = (WaveFragment)waveFragment.GetComponent(typeof(WaveFragment));

           	interfaz.setDireccion(new Vector3 (Mathf.Cos(angle), Mathf.Sin(angle), 0)); 

			interfaz.establecerPropiedades (velocidad,fuerzaImpulso,proporcionDistanciaTamanio,tiempoDisipacionDistancia);
			if (previoInterfaz == null) {
				primerInterfaz = interfaz;
			} else {
				previoInterfaz.setPareja (interfaz);
			}

		   	previoInterfaz = interfaz;
        }
		primerInterfaz.setPareja (interfaz);

		this.ondasLanzadas++;
		this.instanteUltimaOndaLanzada = Time.time;


	}
	

}
