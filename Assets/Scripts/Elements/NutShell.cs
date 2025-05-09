﻿using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BallType
{
    normal,
    killer,
    dumper,
    banger,
}

[System.Serializable]
public class BallData
{
    public SerializableVector3 position = new SerializableVector3();
    public SerializableVector2 scale = new SerializableVector2();
    public float rotation = 0;
    public SerializableVector2 initialVelocity = new SerializableVector2(0, 0);
}

public class NutShell : LevelElementBase
{
    private Rigidbody2D rigidBody;
    public float peso = 8;
    private AudioSource sound;
    private Vector2 currentPosition = new Vector2();
    private Vector2 wavesAcumulatedForce = new Vector2();
    public GameObject sail;
    public BallType ballType = BallType.normal;

    public static List<NutShell> InGameNutShells = new List<NutShell>();

    public override string Type()
    {
        return "NutShell";
    }
    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        InGameNutShells.Add(this);

        //sound = GetComponent<AudioSource> ();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        // rigidBody.velocity = rigidBody.velocity + wavesAcumulatedForce * (1 - Vector2.Dot(rigidBody.velocity.normalized, wavesAcumulatedForce.normalized));
        rigidBody.AddForce(wavesAcumulatedForce);

        wavesAcumulatedForce.x = 0;
        wavesAcumulatedForce.y = 0;

        currentPosition = new Vector2(transform.position.x, transform.position.y);
        OrientarHaciaDireccion();

    }

    public void SelfDestroy()
    {
        InGameNutShells.Remove(this);
        Destroy(gameObject);
    }

    public static bool AnyAlive()
    {
        return InGameNutShells.Count > 0;
    }
    public static void Reset()
    {
        InGameNutShells.ForEach((nut) =>
        {
            Destroy(nut);
        });
        InGameNutShells.Clear();
    }
    void OrientarHaciaDireccion()
    {
        if (rigidBody.velocity.x != 0 || rigidBody.velocity.y != 0)
        {
            float anguloPropio = gameObject.transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
            Vector2 direccionActual = rigidBody.velocity.normalized;
            Vector2 orientacion = new Vector2(Mathf.Cos(anguloPropio), Mathf.Sin(anguloPropio));

            float anguloDiferencia = Vector2.Angle(orientacion, direccionActual);
            Vector3 cross = Vector3.Cross(orientacion, direccionActual);

            if (cross.z < 0)
            {
                anguloDiferencia = -1 * anguloDiferencia;
            }
            //float targetAngle = (sail.transform.rotation.eulerAngles.z * 2 + anguloDiferencia) / 3;
            //sail.transform.rotation = Quaternion.Euler(0, 0, -anguloDiferencia);
            float torque = (float)(anguloDiferencia / (peso * Mathf.PI));

            rigidBody.AddTorque(torque);
        }

    }

    internal void AddWaveFragmentForce(Vector2 fragmentPosition, Vector2 direction)
    {
        float fragmentDistance = (currentPosition - fragmentPosition).sqrMagnitude;

        if (rigidBody != null && fragmentDistance < 0.1f)
        {
            float maxSpeed = 1.2f;
            float dot = Vector2.Dot(direction.normalized, rigidBody.velocity.normalized);
            float velocityRatio = rigidBody.velocity.magnitude / maxSpeed;
            float forceFactor = 1 - Mathf.Clamp(dot * velocityRatio, -0.2f, 1);
            // reduce force by how alike it is to the current direction and how close is current velocity to max velocity
            //wavesAcumulatedForce += direction / (0.5f + fragmentDistance/2);
            Vector2 adjustedForce = direction * forceFactor / (0.5f + fragmentDistance / 2);
            wavesAcumulatedForce += adjustedForce;
        }
    }

    public override ElementData AsLevelData()
    {
        return new ElementData
        {
            type = this.Type(),
            data = JsonConvert.SerializeObject(new BallData()
            {
                position = new SerializableVector3(this.transform.position),
                scale = new SerializableVector2(this.transform.localScale),
                rotation = this.transform.eulerAngles.z,
                initialVelocity = new SerializableVector2()
            })
        };
    }

    public override void LoadFromLevelData(ElementData elementData)
    {
        BallData data = JsonUtility.FromJson<BallData>(elementData.data);
        this.transform.position = data.position.ToVector3();
        this.transform.localScale = data.scale.ToVector2();
        this.transform.rotation = Quaternion.Euler(0, 0, data.rotation);

    }

    public override void SetInert()
    {
        GetComponent<CapsuleCollider2D>().enabled = false;
    }
}
