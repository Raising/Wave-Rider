using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Onda : MonoBehaviour {	
	private float velocidad;
	private float fuerza;
	private float alcanceMaximo; //TODO TENDRIAMOS QUE TENER UN VECTOR CON LA POSICION INICIAL, OTRO CON LA ACTUAL Y EN CADA UPDATE CALCULAR LA DISTANCIA Y VER SI SUPERA A ESTA VARIABLE QUE TAMBIEN SERIA UN VECTOR

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		PropagarOnda();
		DestruirSiAlcanceMaximo();
	}

	private void PropagarOnda() {
		//TODO
	}

	private void DestruirSiAlcanceMaximo() {
		//TODO
	}

	void OnTriggerEnter2D(Collider2D objetoColisionado) {
		Vector2 Direccion = DireccionEntreCentroYObjeto();	
		Rigidbody2D rigiObjetoColisionado = objetoColisionado.GetComponent<Rigidbody2D>();
		//rigiObjetoColisionado.AddForce(Direccion.Normalize * ( this.fuerza )); //TODO
	}

	private Vector2 DireccionEntreCentroYObjeto() {
		Vector2 direccion = new Vector2(0,0); //TODO;

		return direccion;
	}
}


