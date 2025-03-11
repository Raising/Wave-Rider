using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waveBelt : MonoBehaviour
{
    public Vector2 force = new Vector2(0, 1f); // Modify the force vector as needed
    private Vector2 rotatedForce = new Vector2(0, 0);
    private Shapes.Line line;

    private void OnEnable()
    {
        line = GetComponentInChildren<Shapes.Line>();
        Quaternion rotation = transform.rotation;

        // Rotate the vector
        rotatedForce = rotation * force;
    }
    private void Update()
    {
        line.DashOffset += force.y * 0.005f;
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        // Check if the collider has a Rigidbody2D component
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Apply a constant force
            rb.AddForce(rotatedForce);
        }
    }
}