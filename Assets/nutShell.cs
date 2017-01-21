using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nutShell : MonoBehaviour {
	private Rigidbody2D rigidBody;
	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		OrientarHaciaDireccion();
	}

	void OnTriggerEnter2D(Collider2D collider) {
		WaveFragment interfaz = (WaveFragment)collider.GetComponent(typeof(WaveFragment));
		float impulso = interfaz.getImpulso ();
		Vector3 direccion = interfaz.getDireccion ();
		rigidBody.AddForce (direccion * impulso);
	}

	void OrientarHaciaDireccion() {
		
	}
}
