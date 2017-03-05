using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nutShell : MonoBehaviour {
	private Rigidbody2D rigidBody;
	public float peso = 8;
	private AudioSource sound;
	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody2D>();
		//sound = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate () {
		OrientarHaciaDireccion();
	}

	void OnTriggerEnter2D(Collider2D collider) {
		Debug.Log ("pushed by wave");

		if (collider.tag == "Wave") {
			RecibirimpulsoDeOla (collider);
		}
	}
	void OnBecameInvisible(){
		gameOver ();
	}
	void gameOver (){

		AudioManager.Instance.playSound("Loose.wav");
		GameManager.Instance.loseLevel ();
	}

	void RecibirimpulsoDeOla (Collider2D collider){
		WaveSector interfaz = (WaveSector)collider.GetComponent(typeof(WaveSector));
		//TODO
		float impulso = interfaz.getImpulso ();
		Vector2 direccion = interfaz.getDireccion (new Vector2(transform.position.x,transform.position.y) );
		rigidBody.AddForce (direccion * impulso);

	}

	void OrientarHaciaDireccion() {
		if (rigidBody.velocity.x > 0 || rigidBody.velocity.y > 0) {
			float anguloPropio = gameObject.transform.rotation.eulerAngles.z * Mathf.Deg2Rad ;
			Vector2 direccionActual = rigidBody.velocity.normalized;
			Vector2 orientacion = new Vector2 (Mathf.Cos (anguloPropio), Mathf.Sin (anguloPropio));

			float anguloDiferencia = Vector2.Angle(orientacion , direccionActual);
			Vector3 cross = Vector3.Cross(orientacion, direccionActual);

			if (cross.z < 0) {
				anguloDiferencia = -1 * anguloDiferencia;
			}
				
			float torque = (float)(anguloDiferencia / (peso * Mathf.PI));

			rigidBody.AddTorque (torque);
		}

	}
}
