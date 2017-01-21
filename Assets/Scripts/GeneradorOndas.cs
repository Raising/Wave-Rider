using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradorOndas : MonoBehaviour {
	[SerializeField]
	private int numeroOndas;

	private float tiempoEntreOnda;
	private float fuerzaImpulso;
	private float delayInicial;
	private float instanteCreacionGeneradorOndas;
	private float instanteUltimaOndaLanzada;

	private int ondasLanzadas;

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
		return this.ondasLanzadas < this.numeroOndas;
	}

	private bool NingunaOndaHaSidoLanzada() {
		return this.ondasLanzadas == 0;
	} 

	private void GeneraPrimeraOndaTrasDelay() {
		if((Time.time - this.instanteCreacionGeneradorOndas) >= this.delayInicial) {
			GeneraOnda();
		}
	}

	private void LanzarOndaTrasFrecuencia() {
		if((Time.time - this.instanteUltimaOndaLanzada >= this.tiempoEntreOnda)) {
			GeneraOnda();
		}
	}

	private void GeneraOnda() {
		//TODO MAÑANA LO HABLAMOS

		this.instanteUltimaOndaLanzada = Time.time;
	}
	

}
