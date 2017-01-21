using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradorOndas : MonoBehaviour {
	[SerializeField]
	private int numeroOndas;
	[SerializeField]
	private float tiempoEntreOnda;
	private float fuerzaImpulso;
	[SerializeField]
	private float delayInicial;
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
		if(QuedanOndasPorEmitir()) {
			if(NingunaOndaHaSidoLanzada()) {
				GeneraPrimeraOndaTrasDelay();
			} else {
				LanzarOndaTrasFrecuencia();
			}
		}
	}

	private bool QuedanOndasPorEmitir() {
		return this.ondasLanzadas <= this.numeroOndas;
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
		for (int i = 0; i < numeroFragmentoPorOnda; i++) {
           float angle = i * Mathf.PI * 2 / numeroFragmentoPorOnda;
	       GameObject waveFragment = Instantiate(waveFragmentPrefab, gameObject.transform.position, Quaternion.identity);
           WaveFragment interfaz = (WaveFragment)waveFragment.GetComponent(typeof(WaveFragment));
           interfaz.setDireccion(new Vector3 (Mathf.Cos(angle), Mathf.Sin(angle), 0));
		   
        }
		this.ondasLanzadas++;
		this.instanteUltimaOndaLanzada = Time.time;
	}
	

}
