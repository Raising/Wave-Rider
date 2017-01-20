using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradorOndas : MonoBehaviour {
	[SerializeField]
	private int numeroOndasRestantes;

	private float tiempoEntreOnda;
	private float fuerzaImpulso;
	private float delayInicial;
	private float instanteCreacionGeneradorOndas;
	private float instanteUltimaOndaLanzada;

	private int ondasLanzadas;

	public int NumeroOndasRestantes {
		get {
			return this.numeroOndasRestantes;
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
		
	}

	private bool HeLanzadoAlgunaOnda() {
			

		return this.ondasLanzadas == 0;
	} 
	

}
