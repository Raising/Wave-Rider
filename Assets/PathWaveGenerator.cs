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
	public static int textureWidth = 1024;
	public static int textureHeight = 600;

	private Color32[] colorMatrix = new Color32[textureWidth*textureHeight];
	private Color32[] resetColorArray = new Color32[textureWidth*textureHeight];
	private Color32[] mediumDot = new Color32[9];
	public int pathsAmount = 10;

	private PathWave[] paths = null;




	// Use this for initialization
	void Start () {
		this.instanteCreacionGeneradorOndas = 0;
		this.ondasLanzadas = 0;
		sound = GetComponent<AudioSource> ();

		paths = generatePaths ();
		SpriteRenderer waveRenderer = waveDisplayObject.GetComponent<SpriteRenderer> ();
		waveRenderer.sprite = createWavesprite ();
		resetColorArray = setResetTexture ();
	}


	void FixedUpdate () {
		instanteCreacionGeneradorOndas += Time.deltaTime;
		if (instanteCreacionGeneradorOndas > 10) {
			instanteCreacionGeneradorOndas -= tiempoEntreOnda;
		}
		calculateWavePositions ();
	}

	private void calculateWavePositions(){
		float iterationTime = 0;
		PathWave actualRute = null;

		waveTexture.SetPixels32( resetColorArray );
		for (int i = 0; i < pathsAmount; i++) {
			actualRute = paths [i];
			iterationTime = instanteCreacionGeneradorOndas;
			while (iterationTime > 0) {
				Vector2 position = actualRute.setInfluenceInPosition (iterationTime);
				int X = textureWidth/2 + (int)(position.x * 50);
				int Y = textureHeight/2 + (int)(position.y * 50);
				X = Mathf.Min (Mathf.Max (1, X), textureWidth - 1);
				Y = Mathf.Min (Mathf.Max (1, Y), textureHeight - 1);

				drawMediumPoint (X,Y);
				iterationTime -= tiempoEntreOnda;
			}
		}
		waveTexture.Apply ();


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

	private Sprite createWavesprite(){
		waveTexture = new Texture2D(textureWidth,textureHeight);
		return Sprite.Create(waveTexture, new Rect(0.0f,0.0f,waveTexture.width,waveTexture.height), new Vector2(0.5f,0.5f), 100.0f);
	}

	private Color32[] setResetTexture (){
		Color32[] resetColorArrayTemp =  new Color32[textureWidth*textureHeight];


		for(var i = 0; i < textureWidth*textureHeight ; ++i){
			resetColorArrayTemp[i] = Color.clear;
		}

		Color color = Color.red;
		Color color2 = Color.magenta;

		mediumDot [0] = color;
		mediumDot [1] = color2;
		mediumDot [2] = color;
		mediumDot [3] = color2;
		mediumDot [4] = color2;
		mediumDot [5] = color2;
		mediumDot [6] = color;
		mediumDot [7] = color2;
		mediumDot [8] = color;

		return resetColorArrayTemp;
	}

	void drawMediumPoint(int X,int Y){
		if (X <= 0 ) {X = 1;}
		if (Y <= 0 ) {Y = 1;}
		if (X >= (textureWidth-2)  ) {X = textureWidth-3;}
		if (Y >= (textureHeight-2) ) {Y = textureHeight-3;}
		waveTexture.SetPixels32(X-1,Y-1,3,3,mediumDot);
		//waveTexture.SetPixel (X,Y,Color.green);

	}




}