using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveFragment : MonoBehaviour {
	[SerializeField]
	public Vector3 direccion = new Vector3(1,0,0); 
	public float velocidad = 1f;
	public float impulso = 3;
	private float distanciaRecorrida = 0;
	[SerializeField]
	private float proporcionDistanciaTamanio = 0.14f;
	[SerializeField]
	private float tiempoDisipacionDistancia = 5;
	[SerializeField]
	private float anguloBasePareja = 0;
	[SerializeField]
	private WaveFragment pareja;
	private GameObject hijoVisibilidad = null;
	private Rigidbody2D thisrigidbody = null;
	private bool visible = true;
	// Use this for initialization
	void Start () {
		hijoVisibilidad = transform.FindChild ("waveFragmentVision").gameObject;
		thisrigidbody = GetComponent<Rigidbody2D> ();
		thisrigidbody.AddForce (direccion);
	}
	
	// Update is called once per frame
	void Update () {
		Avanzar();
		EstablecerVisibilidad ();
	}
	void OnCollisionEnter2D(Collision2D collision){
	//	Debug.Log ("collision");

	}
	void Avanzar () {
		float tiempo = distanciaRecorrida / velocidad;
		if (tiempo > tiempoDisipacionDistancia){
			Destroy(gameObject);
		}
		thisrigidbody.transform.position += Time.deltaTime * velocidad * direccion;
		distanciaRecorrida += velocidad * Time.deltaTime;
		hijoVisibilidad.transform.localScale = new Vector3 (distanciaRecorrida * proporcionDistanciaTamanio, 1 * ((tiempoDisipacionDistancia - tiempo) /tiempoDisipacionDistancia), 0);

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

	public Vector2 getDireccion2D(){
		return new Vector2 (direccion.x, direccion.y);
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
			if (visible == true) {
				redrawWaveFragment ();	
			}
			if (visible && AnguloConPareja () > 4.1f * anguloBasePareja) {
				setVisible (false);
			} else if (!visible && AnguloConPareja () <= 4.1f * anguloBasePareja) {
				setVisible (true);
			}
		}
	}

	private void setVisible (bool visibilidad){
		visible = visibilidad;
		//Handles.DrawLine(p0, p1);
		hijoVisibilidad.SetActive ( visibilidad);
		if (visible == true) {
			redrawWaveFragment ();	
		}
	}

	public float AnguloConPareja () {
		return Mathf.Abs(pareja.getAngulo() - getAngulo());
	}

	public Vector2 getPosition2D () {
		return 	new Vector2 (gameObject.transform.position.x, gameObject.transform.position.y);
	}

	private void redrawWaveFragment () {
		
		Vector2 startPoint = getPosition2D ();
		Vector2 endPoint = pareja.getPosition2D ();
		Vector2 crossPoint = BezierMidPoint();


		Debug.DrawLine (startPoint,crossPoint,Color.green);
		Debug.DrawLine (endPoint,crossPoint,Color.green);
		Debug.DrawLine (startPoint,startPoint + getDireccion2D(),Color.red);
		Debug.DrawLine (endPoint,endPoint + pareja.getDireccion2D(),Color.red);


	}

	private Vector2 BezierMidPoint () {
		Vector2 startPoint = getPosition2D ();
		Vector2 endPoint = pareja.getPosition2D ();


		Vector2 direccionA = getDireccion2D ();
		Vector2 direccionB = pareja.getDireccion2D ();

		Vector2 startVector = new Vector2 ( -1 * direccionA.y, direccionA.x);
		Vector2 endVector = new Vector2 ( direccionB.y, -1 * direccionB.x);

		float multiplicadorVectorEnd =
			((startVector.x * (startPoint.y - endPoint.y)) - (startVector.y * (startPoint.x - endPoint.x))) /
			((endVector.y * startVector.x) - (startVector.y * endVector.x));

		return endPoint + multiplicadorVectorEnd * endVector;
	}
}