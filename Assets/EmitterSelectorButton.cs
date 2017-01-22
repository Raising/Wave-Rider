using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitterSelectorButton : MonoBehaviour {
	public GameObject waveEmitter;

	[SerializeField]
	private int ammo = 2; 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public GameObject getEmitter () {
		return waveEmitter;
	}

	public void reduceAmmo () {
		ammo -= 1;
		if (ammo <= 0) {
			hide ();
			waveEmitter = null;
		}
	}

	public void hide (){
		transform.localScale = transform.localScale / 1.25f;
		transform.position += Vector3.left * 0.7f;
	}

	public void deselect(){
	}

	public void select (){
	}

}
