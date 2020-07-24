using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathWaveGenerator : MonoBehaviour {
	[SerializeField]
	private int WaveRepetitions;
	[SerializeField]
	private float RepetitionDelay;
	[SerializeField]
	private float InitialDelay;
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

    private float LastTimeToEmitWave = 0;
    private float MaxWaveDuration = 10;
    private Vector2 Direction;

    internal void SetDireciton(Vector2 direction)
    {
        Direction = direction.normalized *  Mathf.Lerp(0, 1f, Mathf.InverseLerp(0.00f, 8, Mathf.Min(8, direction.magnitude))); 
    }

    private int pathsAmount = 128;

	private PathWave[] paths = null;




	// Use this for initialization
	void Start () {
        RepetitionDelay = RepetitionDelay <= 0 ? 10 : RepetitionDelay;
        instanteCreacionGeneradorOndas = 0.001f - InitialDelay;
        LastTimeToEmitWave = -0.001f - InitialDelay - ((WaveRepetitions-1) * RepetitionDelay);
		sound = GetComponent<AudioSource> ();

		paths = generatePaths ();
		//SpriteRenderer waveRenderer = waveDisplayObject.GetComponent<SpriteRenderer> ();
		//waveRenderer.sprite = createWavesprite ();
		//resetColorArray = setResetTexture ();
	}


	void FixedUpdate () {
		instanteCreacionGeneradorOndas += Time.deltaTime;
        LastTimeToEmitWave += Time.deltaTime;


        if (instanteCreacionGeneradorOndas > MaxWaveDuration) {
			instanteCreacionGeneradorOndas -= RepetitionDelay;
        }
        if (instanteCreacionGeneradorOndas > 0) //para esperar al delay
        {
		 calculateWavePositions ();
        }
        if (LastTimeToEmitWave > MaxWaveDuration)
        {
            Destroy(gameObject);
        }
	}

	private void calculateWavePositions(){
		for (int i = 0; i < pathsAmount; i++) {
			paths [i].setInfluenceInPosition (instanteCreacionGeneradorOndas,RepetitionDelay,LastTimeToEmitWave);
		}
	}


	private PathWave[] generatePaths(){
		PathWave[] newPaths = new PathWave[pathsAmount];
		Vector2 position = new Vector2 (transform.position.x, transform.position.y);
		for (int i = 0; i < pathsAmount; i++) {
			float angle = i * Mathf.PI * 2 / pathsAmount;
            Vector2 directionVector = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) + Direction ;
            
            newPaths[i] = new PathWave(position,directionVector); 
		}
		return newPaths;
	}

}