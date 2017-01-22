using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D collider) {
		RedirigirWaveFragment (collider);
		CompensarPosicion (collider);

	}
	void RedirigirWaveFragment (Collider2D collider){
		double anguloPropio = gameObject.transform.rotation.eulerAngles.z / 180 * Mathf.PI ;
		WaveFragment interfaz = (WaveFragment)collider.GetComponent(typeof(WaveFragment));
		double anguloColider = interfaz.getAngulo ();
		float anguloFinal = (float)(anguloColider + (Mathf.PI) + (( anguloPropio - anguloColider ) * 2));
		interfaz.setDireccion(new Vector3 (Mathf.Cos(anguloFinal), Mathf.Sin(anguloFinal), 0));

	
	}
	void CompensarPosicion (Collider2D collider){
		//TODO
	}
}

	
