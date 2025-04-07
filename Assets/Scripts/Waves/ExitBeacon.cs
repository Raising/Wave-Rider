using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ExitType
{
    normal,
    trigger,
}
[System.Serializable]
public class ExitData
{
    public ExitType type = ExitType.normal;
    public Vector2 position = new Vector2();
    public Vector2 scale = new Vector2();
    public float rotation = 0;
    
}
public class ExitBeacon : MonoBehaviour {
	// Use this for initialization
	void Start () {
		//sound = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D (Collider2D collider) {
		if (collider.tag == "NutShip") {
            //sound.Play ();
            AudioManager.Instance.playSound("Win.wav"); //TODO TEMPORALMENTE ROTO, PUES NO ESPERA QUE SE ACABE DE REPRODUCIR EL SONIDO
            collider.GetComponent<nutShell>().SelfDestroy();
            if (!nutShell.AnyAlive())
            {
				EventManager.TriggerEvent("OnLevelCompletion");

            }
		}
	}

    internal ExitData AsLevelData()
    {
        return new ExitData
        {
           position = this.transform.position,
           scale = this.transform.localScale,
            rotation = this.transform.eulerAngles.z
        };
    }

    internal void LoadFromLevelData(ExitData data)
    {
        this.transform.position = data.position;
        this.transform.localScale = data.scale;
        this.transform.rotation = Quaternion.Euler(0, 0, data.rotation);
    }
}
