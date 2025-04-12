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
public class WaterFlow : LevelElementBase
{
    public Vector2 force = new Vector2(0, 1f); // Modify the force vector as needed
    public FlowType flowType = FlowType.lineal;
    private Vector2 rotatedForce = new Vector2(0, 0);
    private Shapes.Line line;

    public override string Type()
    {
        return "WaterFlow";
    }
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

    public override ElementData AsLevelData()
    {
        return new ElementData
        {
            type = this.Type(),
            data = new FlowData
            {
                position = this.transform.position,
                scale = this.transform.localScale,
                rotation = this.transform.eulerAngles.z,
                strength = force.y,
                type = this.flowType,
            }
        };
    }

    public override void LoadFromLevelData(ElementData elementData)
    {
        FlowData data = (FlowData)elementData.data;
        this.transform.position = data.position;
        this.transform.localScale = data.scale;
        this.transform.rotation = Quaternion.Euler(0, 0, data.rotation);
        this.force = new Vector2(0, data.strength);
        this.flowType = FlowType.lineal;
        rotatedForce = this.transform.rotation * force;
    }

    public override void SetInert()
    {
        GetComponent<PolygonCollider2D>().enabled = false;
    }
}