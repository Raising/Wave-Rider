﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveFragment : MonoBehaviour {
	public Vector3 direccion = new Vector3(1,0,0); 
	public float velocidad = 0.1f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Avanzar();
	}

	void Avanzar () {
		gameObject.transform.position += velocidad * direccion;
	}

	public void setDireccion (Vector3 nuevaDireccion) {
		direccion = nuevaDireccion;
		direccion.Normalize();
	}

	public void setVelocidad (float nuevaVelocidad) {
		velocidad = nuevaVelocidad;
	}

}