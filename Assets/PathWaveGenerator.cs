using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathWaveGenerator : MonoBehaviour {
	[SerializeField]
	private int numeroOndas;
	[SerializeField]
	private float tiempoEntreOnda;
	[SerializeField]
	private float delayInicial;
	/*
	[SerializeField]
	private float fuerzaImpulso = 3;
	[SerializeField]
	private float velocidad = 1f;
	[SerializeField]
	private float proporcionDistanciaTamanio = 0.14f;
	[SerializeField]
	private float tiempoDisipacionDistancia = 5;
	*/
	private AudioSource sound; //TODO ESTE SE QUEDA POR AHORA ASI HASTA QUE HAGAMOS LAS SUBCLASES
	private float instanteCreacionGeneradorOndas;
	private float instanteUltimaOndaLanzada;

	private int ondasLanzadas;

	public GameObject waveDisplayObject;
	private Texture2D waveTexture;
	public static int textureWidth = 1920;
	public static int textureHeight = 1080;

	private int pathsAmount = 64;

	private PathWave[] paths = null;




	// Use this for initialization
	void Start () {
		this.instanteCreacionGeneradorOndas = 0;
		this.ondasLanzadas = 0;
		sound = GetComponent<AudioSource> ();

		paths = generatePaths ();
		//SpriteRenderer waveRenderer = waveDisplayObject.GetComponent<SpriteRenderer> ();
		//waveRenderer.sprite = createWavesprite ();
		//resetColorArray = setResetTexture ();
	}


	void FixedUpdate () {
		instanteCreacionGeneradorOndas += Time.deltaTime;
		if (instanteCreacionGeneradorOndas > 10) {
			instanteCreacionGeneradorOndas -= tiempoEntreOnda;
		}
		calculateWavePositions ();
	}

	private void calculateWavePositions(){
		for (int i = 0; i < pathsAmount; i++) {
			paths [i].setInfluenceInPosition (instanteCreacionGeneradorOndas,tiempoEntreOnda);
		}
	}


	private PathWave[] generatePaths(){
		PathWave[] newPaths = new PathWave[pathsAmount];
		Vector2 position = new Vector2 (transform.position.x, transform.position.y);
		for (int i = 0; i < pathsAmount; i++) {
			float angle = i * Mathf.PI * 2 / pathsAmount;

			newPaths[i] = new PathWave(position,new Vector2 (Mathf.Cos(angle), Mathf.Sin(angle))); 
		}
		return newPaths;
	}

}