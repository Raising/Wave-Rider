using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ExitType
{
    normal,
    trigger,
}
[System.Serializable]
public class ExitBeaconData
{
    public ExitType type = ExitType.normal;
    public Vector2 position = new Vector2();
    public Vector2 scale = new Vector2();
    public float rotation = 0;

}
public class ExitBeacon : LevelElementBase
{
    // Use this for initialization

    public override string Type()
    {
        return "ExitBeacon";
    }
    void Start()
    {
        //sound = GetComponent<AudioSource> ();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "NutShip")
        {
            //sound.Play ();
            AudioManager.Instance.playSound("Win.wav"); //TODO TEMPORALMENTE ROTO, PUES NO ESPERA QUE SE ACABE DE REPRODUCIR EL SONIDO
            collider.GetComponent<NutShell>().SelfDestroy();
            if (!NutShell.AnyAlive())
            {
                EventManager.TriggerEvent("OnLevelCompletion");

            }
        }
    }



    public override ElementData AsLevelData()
    {
        return new ElementData
        {
            type = this.Type(),
            data = new ExitBeaconData
            {
                position = this.transform.position,
                scale = this.transform.localScale,
                rotation = this.transform.eulerAngles.z
            }
        };
    }

    public override void LoadFromLevelData(ElementData elementData)
    {
        ExitBeaconData data = (ExitBeaconData)elementData.data;
        this.transform.position = data.position;
        this.transform.localScale = data.scale;
        this.transform.rotation = Quaternion.Euler(0, 0, data.rotation);
    }

    public override void SetInert()
    {
        GetComponent<CircleCollider2D>().enabled = false;
    }
}
