using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nutShell : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    public float peso = 8;
    private AudioSource sound;
    private Vector2 currentPosition = new Vector2();
    private Vector2 wavesAcumulatedForce = new Vector2();

    public static List<nutShell> InGameNutShells = new List<nutShell>();

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


    //void OnBecameInvisible()
    //{
    //    gameOver();
    //}
    //void gameOver()
    //{

    //    AudioManager.Instance.playSound("Loose.wav");
    //    GameManager.Instance.loseLevel();
    //}

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
}
