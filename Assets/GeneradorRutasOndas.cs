using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradorRutasOndas : MonoBehaviour {
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
	private float[,] heightMatrix = new float[1024,600];
	private Vector2[,] gradientMatrix = new Vector2[1024,600];
	Color[] resetColorArray = new Color[1024*600];
	public int numeroRutas = 10;

	private Ruta[] rutes = null;




	// Use this for initialization
	void Start () {
		this.instanteCreacionGeneradorOndas = 0;
		this.ondasLanzadas = 0;
		sound = GetComponent<AudioSource> ();
		//uniqueRute = new Ruta ();
		rutes = generateRutes ();
		SpriteRenderer waveRenderer = waveDisplayObject.GetComponent<SpriteRenderer> ();
		waveRenderer.sprite = createWavesprite ();
		resetColorArray = setResetTexture ();
	}

	// Update is called once per frame
	void Update () {
		instanteCreacionGeneradorOndas += Time.deltaTime;
		float iterationTime = 0;
		Ruta actualRute = null;

		waveTexture.SetPixels( resetColorArray );
		for (int i = 0; i < numeroRutas; i++) {
			actualRute = rutes [i];
			iterationTime = instanteCreacionGeneradorOndas;
			while (iterationTime > 0) {
				Vector2 position = actualRute.getPositionInGivenTime (iterationTime);
				drawMediumPoint (512 + (int)(position.x * 60),300 + (int)(position.y * 60));
				iterationTime -= tiempoEntreOnda;
			}
		}

		waveTexture.Apply ();
	}

	private Ruta[] generateRutes(){
		Ruta[] newRutes = new Ruta[numeroRutas];
		for (int i = 0; i < numeroRutas; i++) {
			float angle = i * Mathf.PI * 2 / numeroRutas;
			newRutes[i] = new Ruta(new Vector2 (Mathf.Cos(angle), Mathf.Sin(angle))); 
		}
		return newRutes;
	}

	private Sprite createWavesprite(){
		waveTexture = new Texture2D(1024,600);
		return Sprite.Create(waveTexture, new Rect(0.0f,0.0f,waveTexture.width,waveTexture.height), new Vector2(0.5f,0.5f), 100.0f);
	}

	private Color[] setResetTexture (){
		Color[] resetColorArrayTemp =  waveTexture.GetPixels();
		int arraySize = resetColorArrayTemp.GetLength (0);

		for(var i = 0; i < arraySize ; ++i){
			resetColorArrayTemp[i] = Color.clear;
		}
		return resetColorArrayTemp;


	}

	void drawMediumPoint(int X,int Y){
		Color color = Color.white;
		waveTexture.SetPixel(X-1,Y-1 , color);
		waveTexture.SetPixel(X,Y-1 , color);
		waveTexture.SetPixel(X+1,Y-1 , color);
		waveTexture.SetPixel(X-1,Y , color);
		waveTexture.SetPixel(X,Y , color);
		waveTexture.SetPixel(X+1,Y , color);
		waveTexture.SetPixel(X-1,Y+1 , color);
		waveTexture.SetPixel(X,Y+1 , color);
		waveTexture.SetPixel(X+1,Y+1 , color);
	}




}