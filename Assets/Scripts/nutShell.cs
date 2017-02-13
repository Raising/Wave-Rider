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
		if (collider.tag == "Wave") {
			RecibirimpulsoDeOla (collider);
		}
	}
	void OnBecameInvisible(){
		gameOver ();
	}
	void gameOver (){
		//sound.Play ();
		AudioManager.Instance.playMusic("Loose.wav"); //TODO TEMPORALMENTE ROTO, PUES NO ESPERA QUE SE ACABE DE REPRODUCIR EL SONIDO
		GameManager.Instance.loseLevel ();
	}

	void RecibirimpulsoDeOla (Collider2D collider){
		WaveFragment interfaz = (WaveFragment)collider.GetComponent(typeof(WaveFragment));
		float impulso = interfaz.getImpulso ();
		Vector3 direccion = interfaz.getDireccion ();
		rigidBody.AddForce (direccion * impulso);
		//MusicManager.Instance.playSound (sound + ".wav");
	}

	void OrientarHaciaDireccion() {
		if (rigidBody.velocity.x > 0 || rigidBody.velocity.y > 0) {
			double anguloPropio = gameObject.transform.rotation.eulerAngles.z / 180 * Mathf.PI ;
			Vector2 direccionActual = rigidBody.velocity.normalized;
			double anguloObjetivo = Mathf.Atan2 (direccionActual.y, direccionActual.x);
			float anguloDiferencia = (float)(anguloObjetivo - anguloPropio);

			if (anguloDiferencia > Mathf.PI) {
				anguloDiferencia -= 2 * Mathf.PI;
			}
			if (anguloDiferencia < Mathf.PI) {
				anguloDiferencia += 2 * Mathf.PI;
			}
			float torque = (float)(anguloDiferencia / (peso * Mathf.PI));
			rigidBody.AddTorque (torque);
		}

	}
}
