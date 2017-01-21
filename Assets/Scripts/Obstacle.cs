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
		double anguloPropio = gameObject.transform.rotation.z;
		WaveFragment interfaz = (WaveFragment)collider.GetComponent(typeof(WaveFragment));
		double anguloColider = interfaz.getAngulo ();
		Debug.Log (anguloPropio);
		float anguloFinal = (float)(anguloColider + (Mathf.PI * 0.5) + (anguloPropio * 2));

		interfaz.setDireccion(new Vector3 (Mathf.Cos(anguloFinal), Mathf.Sin(anguloFinal), 0));

	}
}
