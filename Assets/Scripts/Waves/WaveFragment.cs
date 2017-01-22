using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveFragment : MonoBehaviour {
	public Vector3 direccion = new Vector3(1,0,0); 
	public float velocidad = 1f;
	public float impulso = 3;
	private float distanciaRecorrida = 0;
	[SerializeField]
	private float proporcionDistanciaTamanio = 0.14f;
	[SerializeField]
	private float tiempoDisipacionDistancia = 5;
	private float anguloBasePareja = 0;
	private WaveFragment pareja;
	private GameObject hijoVisibilidad = null;
	private bool visible = true;
	// Use this for initialization
	void Start () {
		hijoVisibilidad = transform.FindChild ("waveFragmentVision").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		Avanzar();
		EstablecerVisibilidad ();
	}

	void Avanzar () {
		float tiempo = distanciaRecorrida / velocidad;
		if (tiempo > tiempoDisipacionDistancia){
			Destroy(gameObject);
		}
		gameObject.transform.position += Time.deltaTime * velocidad * direccion;
		distanciaRecorrida += velocidad * Time.deltaTime;
		gameObject.transform.localScale = new Vector3 (distanciaRecorrida * proporcionDistanciaTamanio, 1 * ((tiempoDisipacionDistancia - tiempo) /tiempoDisipacionDistancia), 0);

	}
	public void establecerPropiedades (float nuevaVelocidad,float nuevaFuerzaImpulso,float nuevaProporcionDistanciaTamanio,float nuevoTiempoDisipacionDistancia){
		velocidad = nuevaVelocidad;
		impulso = nuevaFuerzaImpulso;
		proporcionDistanciaTamanio = nuevaProporcionDistanciaTamanio;
		tiempoDisipacionDistancia = nuevoTiempoDisipacionDistancia;
	}
	public void setDireccion (Vector3 nuevaDireccion) {
		direccion = nuevaDireccion;
		direccion.Normalize();

		transform.rotation = Quaternion.Euler(0, 0, getAngulo ()/Mathf.PI * 180  - 90 );
	}

	public void setVelocidad (float nuevaVelocidad) {
		velocidad = nuevaVelocidad;
	}


	void OnBecameInvisible () {
		Destroy(this.gameObject);
	}

	public float getAngulo () {
		return Mathf.Atan2( direccion.y,direccion.x );
	}

	public float getImpulso () {
		return impulso;
	}

	public Vector3 getDireccion () {
		return direccion;
	}

	public void setPareja (WaveFragment nuevaPareja){
		pareja = nuevaPareja;
		anguloBasePareja = AnguloConPareja();
	}

	public void  EstablecerVisibilidad (){
		if (pareja != null) {
			if (visible && AnguloConPareja () > 2.1f * anguloBasePareja) {
				setVisible (false);
			} else if (!visible && AnguloConPareja () <= 2.1f * anguloBasePareja) {
				setVisible (true);
			}
		}
	}

	private void setVisible (bool visibilidad){
		visible = visibilidad;
		hijoVisibilidad.SetActive ( visibilidad);
	}

	public float AnguloConPareja () {
		return Mathf.Abs(pareja.getAngulo() - getAngulo());
	}
}