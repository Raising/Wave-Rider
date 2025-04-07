using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FlowType
{
    lineal,
    sink,
    emiter,
    pulser,
}


[System.Serializable]
public class FlowData
{
    public FlowType type = FlowType.lineal;
    public Vector2 position = new Vector2();
    public Vector2 scale = new Vector2();
    public float rotation = 0;
    public float strength = 1;
}
public class WaveFlow : MonoBehaviour
{
    public Vector2 force = new Vector2(0, 1f); // Modify the force vector as needed
    public FlowType flowType = FlowType.lineal;
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
        line.DashOffset += force.y * 0.005f * Time.timeScale;
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        // Check if the collider has a Rigidbody2D component
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Apply a constant force
            rb.AddForce(rotatedForce * Time.deltaTime * 30);
        }
    }

    internal FlowData AsLevelData()
    {
        return new FlowData
        {

            position = this.transform.position,
            scale = this.transform.localScale,
            rotation = this.transform.eulerAngles.z,
            strength = force.y,
            type = this.flowType,
    };
    }

    internal void LoadFromLevelData(FlowData data)
    {
        this.transform.position = data.position;
        this.transform.localScale = data.scale;
        this.transform.rotation = Quaternion.Euler(0, 0, data.rotation);
        this.force = new Vector2(0, data.strength);
        this.flowType = FlowType.lineal;
        //Todo restore line size
    }
}